using FeedbackService.Application.DTOs;

namespace FeedbackService.Application.Interfaces;

public interface ISupportTicketService
{
    Task<SupportTicketResponseDto> GetByIdAsync(Guid ticketId, Guid driverId);
    Task<IEnumerable<SupportTicketResponseDto>> GetMyTicketsAsync(Guid driverId);
    Task<SupportTicketResponseDto> CreateAsync(CreateSupportTicketDto dto, Guid driverId);
}
