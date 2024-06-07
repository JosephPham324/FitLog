using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries_.GetUsers;
public class AspNetUserListDTO
{
    [Required]
    public string? Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<AspNetUser, AspNetUserListDTO>();
        }
    }
}
