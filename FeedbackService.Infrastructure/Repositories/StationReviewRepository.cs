using FeedbackService.Application.Interfaces;
using FeedbackService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Infrastructure.Repositories;

public class StationReviewRepository : IStationReviewRepository
{
    private readonly ev_battery_swapContext _context;

    public StationReviewRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<StationReview> GetByIdAsync(Guid reviewId)
    {
        return await _context.StationReviews
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
    }

    public async Task<IEnumerable<StationReview>> GetByDriverIdAsync(Guid driverId)
    {
        return await _context.StationReviews
            .Where(r => r.DriverId == driverId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<StationReview>> GetByStationIdAsync(Guid stationId)
    {
        return await _context.StationReviews
            .Where(r => r.StationId == stationId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<StationReview>> GetAllAsync()
    {
        return await _context.StationReviews
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<StationReview> CreateAsync(StationReview review)
    {
        _context.StationReviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<StationReview> UpdateAsync(StationReview review)
    {
        review.UpdatedAt = DateTime.UtcNow;
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<bool> DeleteAsync(Guid reviewId)
    {
        var review = await GetByIdAsync(reviewId);
        if (review == null)
            return false;

        _context.StationReviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<double> GetAverageRatingByStationIdAsync(Guid stationId)
    {
        var reviews = await _context.StationReviews
            .Where(r => r.StationId == stationId)
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.Rating);
    }
}
