using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Infrastructure.Repositories;

public class SwapRepository : ISwapRepository
{
    private readonly ev_battery_swapContext _context;

    public SwapRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Swap> GetByIdAsync(Guid swapId)
    {
        return await _context.Swaps
            .FirstOrDefaultAsync(s => s.SwapId == swapId);
    }

    public async Task<bool> IsSwapOwnedByDriverAsync(Guid swapId, Guid driverId)
    {
        return await _context.Swaps
            .AnyAsync(s => s.SwapId == swapId && s.DriverId == driverId);
    }
}
