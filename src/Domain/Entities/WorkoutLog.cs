using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class WorkoutLog : BaseAuditableEntity
{
    #region Inherited Properties
    //public int Id { get; set; }

    //public DateTimeOffset Created { get; set; }

    //public string? CreatedBy { get; set; }

    //public DateTimeOffset LastModified { get; set; }

    //public string? LastModifiedBy { get; set; }
    #endregion

    public string? Note { get; set; }

    public TimeOnly? Duration { get; set; }
    
    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}
