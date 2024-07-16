using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Constants;
public abstract class EnrollmentStatuses
{
    public const string Enrolled = nameof(Enrolled);
    public const string Completed = nameof(Completed);
    public const string Dropped = nameof(Dropped);
    public const string InProgress = nameof(InProgress);
    public const string Paused = nameof(Paused);
}
