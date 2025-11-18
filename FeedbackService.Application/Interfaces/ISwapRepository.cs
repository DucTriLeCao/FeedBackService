using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Interfaces;

public interface ISwapRepository
{
    Task<Swap> GetByIdAsync(Guid swapId);
    Task<bool> IsSwapOwnedByDriverAsync(Guid swapId, Guid driverId);
}
