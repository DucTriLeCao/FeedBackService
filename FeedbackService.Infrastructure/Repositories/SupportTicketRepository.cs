using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Infrastructure.Repositories;

public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly ev_battery_swapContext _context;

    public SupportTicketRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<SupportTicket> GetByIdAsync(Guid ticketId)
    {
        return await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }

    public async Task<SupportTicket> GetByTicketNumberAsync(string ticketNumber)
    {
        return await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber);
    }

    public async Task<IEnumerable<SupportTicket>> GetByDriverIdAsync(Guid driverId)
    {
        return await _context.SupportTickets
            .Where(t => t.DriverId == driverId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupportTicket>> GetByStationIdAsync(Guid stationId)
    {
        return await _context.SupportTickets
            .Where(t => t.StationId == stationId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupportTicket>> GetByStatusAsync(string status)
    {
        return await _context.SupportTickets
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupportTicket>> GetAllAsync()
    {
        return await _context.SupportTickets
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<SupportTicket> CreateAsync(SupportTicket ticket)
    {
        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<SupportTicket> UpdateAsync(SupportTicket ticket)
    {
        ticket.UpdatedAt = DateTime.UtcNow;
        _context.Entry(ticket).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<bool> DeleteAsync(Guid ticketId)
    {
        var ticket = await GetByIdAsync(ticketId);
        if (ticket == null)
            return false;

        _context.SupportTickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }
}
