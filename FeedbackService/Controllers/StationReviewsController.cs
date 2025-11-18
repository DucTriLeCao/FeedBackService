using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FeedbackService.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize(Roles = "driver")]
public class StationReviewsController : ControllerBase
{
    private readonly IStationReviewService _service;
    private readonly IDriverRepository _driverRepository;

    public StationReviewsController(IStationReviewService service, IDriverRepository driverRepository)
    {
        _service = service;
        _driverRepository = driverRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StationReviewResponseDto>>> GetMyReviews()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var reviews = await _service.GetMyReviewsAsync(driver.DriverId);
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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var review = await _service.GetByIdAsync(id, driver.DriverId);
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

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var review = await _service.CreateAsync(dto, driver.DriverId);
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

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var review = await _service.UpdateAsync(id, dto, driver.DriverId);
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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var result = await _service.DeleteAsync(id, driver.DriverId);
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