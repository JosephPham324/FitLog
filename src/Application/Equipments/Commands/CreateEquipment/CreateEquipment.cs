using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Domain.Entities;

namespace FitLog.Application.Equipments.Commands.CreateEquipment;
public record CreateEquipmentCommand : IRequest<int>
{
    public string? EquipmentName { get; set; }

    public string? ImageUrl { get; set; }
}
public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Equipment
        {
            EquipmentName = request.EquipmentName,
            ImageUrl = request.ImageUrl
        };

        _context.Equipment.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.EquipmentId;
    }
}
