using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Interfaces;

public interface IStationReviewRepository
{
    Task<StationReview> GetByIdAsync(Guid reviewId);
    Task<IEnumerable<StationReview>> GetByDriverIdAsync(Guid driverId);
    Task<IEnumerable<StationReview>> GetByStationIdAsync(Guid stationId);
    Task<IEnumerable<StationReview>> GetAllAsync();
    Task<StationReview> CreateAsync(StationReview review);
    Task<StationReview> UpdateAsync(StationReview review);
    Task<bool> DeleteAsync(Guid reviewId);
    Task<double> GetAverageRatingByStationIdAsync(Guid stationId);
}
