namespace FeedbackService.Application.DTOs;

public class CreateStationReviewDto
{
    public Guid StationId { get; set; }
    public Guid? SwapId { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; }
    public int? ServiceSpeedRating { get; set; }
    public int? StaffRating { get; set; }
    public int? FacilityRating { get; set; }
}

public class UpdateStationReviewDto
{
    public int Rating { get; set; }
    public string ReviewText { get; set; }
    public int? ServiceSpeedRating { get; set; }
    public int? StaffRating { get; set; }
    public int? FacilityRating { get; set; }
}

public class StationReviewResponseDto
{
    public Guid ReviewId { get; set; }
    public Guid DriverId { get; set; }
    public Guid StationId { get; set; }
    public Guid? SwapId { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; }
    public int? ServiceSpeedRating { get; set; }
    public int? StaffRating { get; set; }
    public int? FacilityRating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
