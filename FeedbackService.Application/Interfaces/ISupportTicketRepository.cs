using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Interfaces;

public interface ISupportTicketRepository
{
    Task<SupportTicket> GetByIdAsync(Guid ticketId);
    Task<SupportTicket> GetByTicketNumberAsync(string ticketNumber);
    Task<IEnumerable<SupportTicket>> GetByDriverIdAsync(Guid driverId);
    Task<IEnumerable<SupportTicket>> GetByStationIdAsync(Guid stationId);
    Task<IEnumerable<SupportTicket>> GetByStatusAsync(string status);
    Task<IEnumerable<SupportTicket>> GetAllAsync();
    Task<SupportTicket> CreateAsync(SupportTicket ticket);
    Task<SupportTicket> UpdateAsync(SupportTicket ticket);
    Task<bool> DeleteAsync(Guid ticketId);
}
