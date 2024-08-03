using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogDetails
{
    public record GetWorkoutLogDetailsQuery : IRequest<WorkoutLogDetailsDto>
    {
        public string UserId { get; init; } = string.Empty;
        public int WorkoutLogId { get; init; }
    }

    public class GetWorkoutLogDetailsQueryValidator : AbstractValidator<GetWorkoutLogDetailsQuery>
    {
        public GetWorkoutLogDetailsQueryValidator()
        {
            RuleFor(v => v.WorkoutLogId).NotEmpty().WithMessage("WorkoutLogId is required.");
        }
    }

    public class GetWorkoutLogDetailsQueryHandler : IRequestHandler<GetWorkoutLogDetailsQuery, WorkoutLogDetailsDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetWorkoutLogDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WorkoutLogDetailsDto> Handle(GetWorkoutLogDetailsQuery request, CancellationToken cancellationToken)
        {
            var workoutLog = await _context.WorkoutLogs
                .Include(wl => wl.ExerciseLogs)
                .ThenInclude(el => el.Exercise)
                .FirstOrDefaultAsync(wl => wl.Id == request.WorkoutLogId, cancellationToken);
           

            if (workoutLog == null)
            {
                throw new NotFoundException(nameof(WorkoutLog), request.WorkoutLogId + "");
            }

            if (workoutLog.CreatedBy != request.UserId)
            {
                throw new UnauthorizedAccessException();
            }

            return _mapper.Map<WorkoutLogDetailsDto>(workoutLog);
        }
    }

    public class WorkoutLogDetailsDto
    {
        public int Id { get; set; }
        public string WorkoutLogName { get; set; } = string.Empty;
        public string? Note { get; set; }
        public TimeOnly? Duration { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public List<ExerciseLogDTO> ExerciseLogs { get; set; } = new List<ExerciseLogDTO>();

        public class Mapping : AutoMapper.Profile
        {
            public Mapping()
            {
                CreateMap<WorkoutLog, WorkoutLogDetailsDto>();
                CreateMap<ExerciseLog, ExerciseLogDTO>()
                    .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.ExerciseName : "Unknown exercise"));
            }
        }
    }
}
