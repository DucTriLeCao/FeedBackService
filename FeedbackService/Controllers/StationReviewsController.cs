using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackService.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize(Roles = "driver")]
public class StationReviewsController : ControllerBase
{
    private readonly IStationReviewService _service;

    public StationReviewsController(IStationReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StationReviewResponseDto>>> GetMyReviews()
    {
        try
        {
            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var reviews = await _service.GetMyReviewsAsync(driverId);
            return Ok(reviews);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StationReviewResponseDto>> GetById(Guid id)
    {
        try
        {
            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var review = await _service.GetByIdAsync(id, driverId);
            if (review == null)
                return NotFound(new { message = "Review not found or access denied" });

            return Ok(review);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<StationReviewResponseDto>> Create([FromBody] CreateStationReviewDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var review = await _service.CreateAsync(dto, driverId);
            return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StationReviewResponseDto>> Update(Guid id, [FromBody] UpdateStationReviewDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var review = await _service.UpdateAsync(id, dto, driverId);
            if (review == null)
                return NotFound(new { message = "Review not found or access denied" });

            return Ok(review);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var result = await _service.DeleteAsync(id, driverId);
            if (!result)
                return NotFound(new { message = "Review not found or access denied" });

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

[ApiController]
[Route("api/stations/{stationId}/reviews")]
public class StationPublicReviewsController : ControllerBase
{
    private readonly IStationReviewService _service;

    public StationPublicReviewsController(IStationReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StationReviewResponseDto>>> GetStationReviews(Guid stationId)
    {
        try
        {
            var reviews = await _service.GetByStationIdAsync(stationId);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("average-rating")]
    public async Task<ActionResult<object>> GetAverageRating(Guid stationId)
    {
        try
        {
            var averageRating = await _service.GetAverageRatingByStationIdAsync(stationId);
            return Ok(new { stationId, averageRating });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}