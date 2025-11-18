using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Services;

public class SupportTicketService : ISupportTicketService
{
    private readonly ISupportTicketRepository _repository;

    public SupportTicketService(ISupportTicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<SupportTicketResponseDto> GetByIdAsync(Guid ticketId, Guid driverId)
    {
        var ticket = await _repository.GetByIdAsync(ticketId);
        if (ticket == null || ticket.DriverId != driverId)
            return null;

        return MapToResponseDto(ticket);
    }

    public async Task<IEnumerable<SupportTicketResponseDto>> GetMyTicketsAsync(Guid driverId)
    {
        var tickets = await _repository.GetByDriverIdAsync(driverId);
        return tickets.Select(MapToResponseDto);
    }

    public async Task<SupportTicketResponseDto> CreateAsync(CreateSupportTicketDto dto, Guid driverId)
    {
        var ticket = new SupportTicket
        {
            TicketId = Guid.NewGuid(),
            TicketNumber = GenerateTicketNumber(),
            DriverId = driverId,
            StationId = dto.StationId,
            SwapId = dto.SwapId,
            Category = dto.Category,
            Priority = "medium",
            Subject = dto.Subject,
            Description = dto.Description,
            Status = "open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(ticket);
        return MapToResponseDto(created);
    }

    private SupportTicketResponseDto MapToResponseDto(SupportTicket ticket)
    {
        if (ticket == null)
            return null;

        return new SupportTicketResponseDto
        {
            TicketId = ticket.TicketId,
            TicketNumber = ticket.TicketNumber,
            DriverId = ticket.DriverId,
            StationId = ticket.StationId,
            SwapId = ticket.SwapId,
            Category = ticket.Category,
            Priority = ticket.Priority,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            AssignedTo = ticket.AssignedTo,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ResolvedAt = ticket.ResolvedAt
        };
    }

    private string GenerateTicketNumber()
    {
        return $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }
}
