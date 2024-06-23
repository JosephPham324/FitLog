using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.Login;
public class LoginResultDTO
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
}
