using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int? UserId { get; set; }

    public string? Bio { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual User? User { get; set; }
}
