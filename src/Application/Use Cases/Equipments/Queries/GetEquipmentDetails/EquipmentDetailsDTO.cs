using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Equipments.Queries.GetEquipmentsList;

public class EquipmentDetailsDTO
{
    public int EquipmentId { get; set; }
    public string? EquipmentName { get; set; }
    public string? ImageUrl { get; set; }

    public class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Equipment, EquipmentDetailsDTO>();
        }
    }
}
