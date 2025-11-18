using FeedbackService.Application.DTOs;
using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;

namespace FeedbackService.Application.Services;

public class StationReviewService : IStationReviewService
{
    private readonly IStationReviewRepository _repository;
    private readonly ISwapRepository _swapRepository;

    public StationReviewService(IStationReviewRepository repository, ISwapRepository swapRepository)
    {
        _repository = repository;
        _swapRepository = swapRepository;
    }

    public async Task<StationReviewResponseDto> GetByIdAsync(Guid reviewId, Guid driverId)
    {
        var review = await _repository.GetByIdAsync(reviewId);
        if (review == null || review.DriverId != driverId)
            return null;

        return MapToResponseDto(review);
    }

    public async Task<IEnumerable<StationReviewResponseDto>> GetMyReviewsAsync(Guid driverId)
    {
        var reviews = await _repository.GetByDriverIdAsync(driverId);
        return reviews.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<StationReviewResponseDto>> GetByStationIdAsync(Guid stationId)
    {
        var reviews = await _repository.GetByStationIdAsync(stationId);
        return reviews.Select(MapToResponseDto);
    }

    public async Task<StationReviewResponseDto> CreateAsync(CreateStationReviewDto dto, Guid driverId)
    {
        ValidateRating(dto.Rating);
        ValidateOptionalRating(dto.ServiceSpeedRating);
        ValidateOptionalRating(dto.StaffRating);
        ValidateOptionalRating(dto.FacilityRating);

        if (dto.SwapId.HasValue)
        {
            var isOwned = await _swapRepository.IsSwapOwnedByDriverAsync(dto.SwapId.Value, driverId);
            if (!isOwned)
                throw new UnauthorizedAccessException("SwapId does not belong to this driver");
        }

        var review = new StationReview
        {
            ReviewId = Guid.NewGuid(),
            DriverId = driverId,
            StationId = dto.StationId,
            SwapId = dto.SwapId,
            Rating = dto.Rating,
            ReviewText = dto.ReviewText,
            ServiceSpeedRating = dto.ServiceSpeedRating,
            StaffRating = dto.StaffRating,
            FacilityRating = dto.FacilityRating,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(review);
        return MapToResponseDto(created);
    }

    public async Task<StationReviewResponseDto> UpdateAsync(Guid reviewId, UpdateStationReviewDto dto, Guid driverId)
    {
        var review = await _repository.GetByIdAsync(reviewId);
        if (review == null || review.DriverId != driverId)
            return null;

        ValidateRating(dto.Rating);
        ValidateOptionalRating(dto.ServiceSpeedRating);
        ValidateOptionalRating(dto.StaffRating);
        ValidateOptionalRating(dto.FacilityRating);

        review.Rating = dto.Rating;
        review.ReviewText = dto.ReviewText;
        review.ServiceSpeedRating = dto.ServiceSpeedRating;
        review.StaffRating = dto.StaffRating;
        review.FacilityRating = dto.FacilityRating;
        review.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(review);
        return MapToResponseDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid reviewId, Guid driverId)
    {
        var review = await _repository.GetByIdAsync(reviewId);
        if (review == null || review.DriverId != driverId)
            return false;

        return await _repository.DeleteAsync(reviewId);
    }

    public async Task<double> GetAverageRatingByStationIdAsync(Guid stationId)
    {
        return await _repository.GetAverageRatingByStationIdAsync(stationId);
    }

    private StationReviewResponseDto MapToResponseDto(StationReview review)
    {
        if (review == null)
            return null;

        return new StationReviewResponseDto
        {
            ReviewId = review.ReviewId,
            DriverId = review.DriverId,
            StationId = review.StationId,
            SwapId = review.SwapId,
            Rating = review.Rating,
            ReviewText = review.ReviewText,
            ServiceSpeedRating = review.ServiceSpeedRating,
            StaffRating = review.StaffRating,
            FacilityRating = review.FacilityRating,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt
        };
    }

    private void ValidateRating(int rating)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5");
    }

    private void ValidateOptionalRating(int? rating)
    {
        if (rating.HasValue && (rating.Value < 1 || rating.Value > 5))
            throw new ArgumentException("Rating must be between 1 and 5");
    }
}
