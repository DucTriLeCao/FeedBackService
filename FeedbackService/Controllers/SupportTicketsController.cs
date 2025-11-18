using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackService.Controllers;

[ApiController]
[Route("api/support-tickets")]
[Authorize(Roles = "driver")]
public class SupportTicketsController : ControllerBase
{
    private readonly ISupportTicketService _service;

    public SupportTicketsController(ISupportTicketService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupportTicketResponseDto>>> GetMyTickets()
    {
        try
        {
            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var tickets = await _service.GetMyTicketsAsync(driverId);
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
            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var ticket = await _service.GetByIdAsync(id, driverId);
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

            var driverId = Guid.Parse(User.FindFirst("driverId")?.Value
                ?? throw new UnauthorizedAccessException("Driver ID not found in token"));

            var ticket = await _service.CreateAsync(dto, driverId);
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
