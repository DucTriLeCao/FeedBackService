namespace FeedbackService.Application.DTOs;

public class CreateSupportTicketDto
{
    public Guid? StationId { get; set; }
    public Guid? SwapId { get; set; }
    public string Category { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
}

public class UpdateSupportTicketDto
{
    public string Status { get; set; }
    public Guid? AssignedTo { get; set; }
}

public class SupportTicketResponseDto
{
    public Guid TicketId { get; set; }
    public string TicketNumber { get; set; }
    public Guid DriverId { get; set; }
    public Guid? StationId { get; set; }
    public Guid? SwapId { get; set; }
    public string Category { get; set; }
    public string Priority { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public Guid? AssignedTo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
