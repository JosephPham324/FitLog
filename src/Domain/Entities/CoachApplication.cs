using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Entities;
public class CoachApplication : BaseAuditableEntity
{
    //public int Id { get; set; }

    public string ApplicantId { get; set; } = "";
    public virtual AspNetUser Applicant { get; set; } = null!;

    public string Status { get; set; } = "Pending";

    public string? StatusReason { get; set; }


    //public DateTime StatusUpdateTime { get; set; }

    //public string? StatusUpdatedById { get; set; }
    public virtual AspNetUser? StatusUpdatedBy { get; set; }
}
