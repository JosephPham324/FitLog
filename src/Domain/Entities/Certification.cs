using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Certification
{
    public int CertificationId { get; set; }

    public int? UserId { get; set; }

    public string? CertificationName { get; set; }

    public DateOnly? CertificationDateIssued { get; set; }

    public DateOnly? CertificationExpirationData { get; set; }

    public virtual User? User { get; set; }
}
