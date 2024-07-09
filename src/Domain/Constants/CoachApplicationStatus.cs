using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Constants;
public abstract class CoachApplicationStatus
{
    public const string Pending = nameof(Pending);
    public const string Approved = nameof(Approved);
    public const string Denied = nameof(Denied);
    public const string Cancelled = nameof(Cancelled);
}
