using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Application.Common.ValidationRules;
public static class ValidationRules
{
    public static bool BeAValidUrl(string? imageUrl)
    {
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out _);
    }

    public static IRuleBuilderOptions<T, TProperty> MustExist<T, TEntity, TProperty>(
       this IRuleBuilderInitial<T, TProperty> ruleBuilder,
       IApplicationDbContext context,
       Func<T, TProperty> propertySelector,
       string entityName,
       params object[] keys
   ) where T : class where TEntity : class
    {
        return ruleBuilder
           .MustAsync((propertyValue, cancellationToken) =>
           {
               return Task.FromResult(Exists<TEntity>(context, keys));
           })
           .WithMessage($"{entityName} with specified key(s) does not exist.");
    }

    private static bool Exists<T>(IApplicationDbContext context, params object[] keys) where T : class
    {
        return context.Set<T>().Find(keys) != null;
    }

    public static bool BeAValidRole(string role)
    {
        var type = typeof(CoachApplicationStatus);
        var validRoles = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                   .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                   .Select(fi => fi?.GetRawConstantValue()?.ToString())
                   .ToHashSet(StringComparer.OrdinalIgnoreCase);
        return validRoles.Contains(role);
    }

    public static bool BeAValidCoachApplicationStatus(string status)
    {
        var type = typeof(CoachApplicationStatus);
        var validStatuses = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                   .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                   .Select(fi => fi?.GetRawConstantValue()?.ToString())
                   .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return validStatuses.Contains(status);
    }

    public static bool ValidTimeFrame(string timeFrame)
    {
        if (string.IsNullOrEmpty(timeFrame))
        {
            return false;
        }

        var normalizedTimeFrame = timeFrame.ToUpperInvariant();
        return normalizedTimeFrame.Equals(TimeFrames.Weekly.ToUpperInvariant()) ||
               normalizedTimeFrame.Equals(TimeFrames.Monthly.ToUpperInvariant()) ||
               normalizedTimeFrame.Equals(TimeFrames.Yearly.ToUpperInvariant());
    }
}
