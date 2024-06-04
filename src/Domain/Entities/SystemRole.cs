using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class SystemRole : AspNetRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDesc { get; set; }
    public override ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
