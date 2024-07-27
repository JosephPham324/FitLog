using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Domain.Entities;

namespace FitLog.Application.Users.Queries.GetUsers;
public class UserListDTO
{
    [Required]
    public string? Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    [JsonPropertyName("IsRestricted")]
    public bool? IsDeleted { get; set; }
    public List<string> Roles { get; set; } = new List<string>();


    private class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<AspNetUser, UserListDTO>()
                    .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToList()));
        }
    }
}
