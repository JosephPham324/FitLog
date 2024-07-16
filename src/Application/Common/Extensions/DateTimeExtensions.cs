using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Domain.Constants;

namespace FitLog.Application.Common.Extensions;
public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    public static DateTime StartOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1);
    }

    public static DateTime StartOfYear(this DateTime dt)
    {
        return new DateTime(dt.Year, 1, 1);
    }

    public static DateTime AddTimeFrame(this DateTime start, string timeFrame)
    {
        switch (timeFrame.ToUpperInvariant())
        {
            case string weekly when weekly == TimeFrames.Weekly.ToUpperInvariant():
                return start.AddDays(7);
            case string monthly when monthly == TimeFrames.Monthly.ToUpperInvariant():
                return start.AddMonths(1);
            case string yearly when yearly == TimeFrames.Yearly.ToUpperInvariant():
                return start.AddYears(1);
            default:
                throw new ArgumentException("Invalid TimeFrame", nameof(timeFrame));
        }
    }
}
