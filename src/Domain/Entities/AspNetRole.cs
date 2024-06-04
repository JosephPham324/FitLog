using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace FitLog.Domain.Entities;

public partial class AspNetRole : IdentityRole<string>
{
    public AspNetRole(string Name) { this.Name = Name;  }
    public string? RoleDesc { get; set; }

    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
