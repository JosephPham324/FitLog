using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class CoachingBooking
{
    public int BookingId { get; set; }

    public string? UserId { get; set; }

    public int? CoachingServiceId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual CoachingService? CoachingService { get; set; }

    public virtual AspNetUser? User { get; set; }
}
