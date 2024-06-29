using System;
using System.Collections.Generic;
using System.Linq;
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
        return role == Roles.Administrator || role == Roles.Member || role == Roles.Coach;
    }
}
