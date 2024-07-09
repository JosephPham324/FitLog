using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Domain.Entities;

namespace FitLog.Application.Equipments.Commands.CreateEquipment;
public record CreateEquipmentCommand : IRequest<Result>
{
    public string? EquipmentName { get; set; }

    public string? ImageUrl { get; set; }
}



public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Equipment
        {
            EquipmentName = request.EquipmentName,
            ImageUrl = request.ImageUrl
        };

        _context.Equipment.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful();
    }
}
