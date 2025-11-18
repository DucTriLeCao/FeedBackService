using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Interfaces;

public interface IDriverRepository
{
    Task<Driver> GetByUserIdAsync(Guid userId);
    Task<Driver> GetByDriverIdAsync(Guid driverId);
}
