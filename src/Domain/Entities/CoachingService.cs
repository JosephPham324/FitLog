using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class CoachingService
{
    public int CoachingServiceId { get; set; }

    public int? CreatedBy { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public int? Duration { get; set; }

    public decimal? Price { get; set; }

    public bool? ServiceAvailability { get; set; }

    public string? AvailabilityAnnouncement { get; set; }

    public virtual ICollection<CoachingBooking> CoachingBookings { get; set; } = new List<CoachingBooking>();

    public virtual User? CreatedByNavigation { get; set; }
}
