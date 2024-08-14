using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Models;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.Login;
public class LoginResultDTO
{
    public bool Success;
    public Result? Result { get; set; }
    public string Token { get; set; } = string.Empty;

    public static LoginResultDTO Failure(IEnumerable<string> errors)
    {
        return new LoginResultDTO
        {
            Success = false,
            Result = Result.Failure(errors),
            Token = string.Empty
        };
    }
    public static LoginResultDTO Successful(string token)
    {
        return new LoginResultDTO
        {
            Success = true,
            Result = Result.Successful(),
            Token = string.Empty
        };
    }
}
