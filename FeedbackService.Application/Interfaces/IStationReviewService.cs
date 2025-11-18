using FeedbackService.Application.DTOs;

namespace FeedbackService.Application.Interfaces;

public interface IStationReviewService
{
    Task<StationReviewResponseDto> GetByIdAsync(Guid reviewId, Guid driverId);
    Task<IEnumerable<StationReviewResponseDto>> GetMyReviewsAsync(Guid driverId);
    Task<IEnumerable<StationReviewResponseDto>> GetByStationIdAsync(Guid stationId);
    Task<StationReviewResponseDto> CreateAsync(CreateStationReviewDto dto, Guid driverId);
    Task<StationReviewResponseDto> UpdateAsync(Guid reviewId, UpdateStationReviewDto dto, Guid driverId);
    Task<bool> DeleteAsync(Guid reviewId, Guid driverId);
    Task<double> GetAverageRatingByStationIdAsync(Guid stationId);
}
