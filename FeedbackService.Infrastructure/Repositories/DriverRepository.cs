using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly ev_battery_swapContext _context;

    public DriverRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Driver> GetByUserIdAsync(Guid userId)
    {
        return await _context.Drivers
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }

    public async Task<Driver> GetByDriverIdAsync(Guid driverId)
    {
        return await _context.Drivers
            .FirstOrDefaultAsync(d => d.DriverId == driverId);
    }
}
