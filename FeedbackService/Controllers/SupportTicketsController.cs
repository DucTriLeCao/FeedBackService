using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FeedbackService.Controllers;

[ApiController]
[Route("api/support-tickets")]
[Authorize(Roles = "driver")]
public class SupportTicketsController : ControllerBase
{
    private readonly ISupportTicketService _service;
    private readonly IDriverRepository _driverRepository;

    public SupportTicketsController(ISupportTicketService service, IDriverRepository driverRepository)
    {
        _service = service;
        _driverRepository = driverRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupportTicketResponseDto>>> GetMyTickets()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var tickets = await _service.GetMyTicketsAsync(driver.DriverId);
            return Ok(tickets);
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
    public async Task<ActionResult<SupportTicketResponseDto>> GetById(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token"));

            var driver = await _driverRepository.GetByUserIdAsync(userId);
            if (driver == null)
                return StatusCode(403, new { message = "Driver profile not found" });

            var ticket = await _service.GetByIdAsync(id, driver.DriverId);
            if (ticket == null)
                return NotFound(new { message = "Ticket not found or access denied" });

            return Ok(ticket);
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
    public async Task<ActionResult<SupportTicketResponseDto>> Create([FromBody] CreateSupportTicketDto dto)
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

            var ticket = await _service.CreateAsync(dto, driver.DriverId);
            return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
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
