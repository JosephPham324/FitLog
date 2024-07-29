using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Constants;
public abstract class ServiceBookingStatus
{
    public static readonly string Pending = "Pending";
    public static readonly string Confirmed = "Confirmed";
    public static readonly string Cancelled = "Cancelled";
    public static readonly string Completed = "Completed";
    public static readonly string Failed = "Failed";

    // You can add additional methods if necessary, for example, to validate statuses.
    public static bool IsValidStatus(string status)
    {
        return status == Pending ||
               status == Confirmed ||
               status == Cancelled ||
               status == Completed ||
               status == Failed;
    }
}
