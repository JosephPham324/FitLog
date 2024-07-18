using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;

namespace FitLog.Application.Equipments.Queries.GetEquipmentsList;

public class EquipmentDTO
{
    public int EquipmentId { get; set; }
    public string? EquipmentName { get; set; }

    public class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<Equipment, EquipmentDTO>();
        }
    }
}
