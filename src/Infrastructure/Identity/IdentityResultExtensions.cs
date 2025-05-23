﻿using FitLog.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Successful()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}
