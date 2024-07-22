using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Mappings;
using FitLog.Application.Common.Models;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitLog.Application.Users.Queries.GetCoachesListWithPagination
{
    public record GetCoachesListWithPaginationQuery : IRequest<PaginatedList<CoachSummaryDTO>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetCoachesListWithPaginationQueryValidator : AbstractValidator<GetCoachesListWithPaginationQuery>
    {
        public GetCoachesListWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }

    public class GetCoachesListWithPaginationQueryHandler : IRequestHandler<GetCoachesListWithPaginationQuery, PaginatedList<CoachSummaryDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCoachesListWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CoachSummaryDTO>> Handle(GetCoachesListWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var coaches = await _context.AspNetUsers
                .Where(u => u.Roles.Any(r => r.Name == Domain.Constants.Roles.Coach))
                    .Include(u => u.Profiles)
                    .Include(u => u.Programs)
                .Select(u => new CoachSummaryDTO(u.Profiles.FirstOrDefault()?? new Domain.Entities.Profile(),u)
                )
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return coaches;
        }
    }
}
