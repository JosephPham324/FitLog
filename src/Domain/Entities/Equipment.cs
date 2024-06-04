using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string? EquipmentName { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
