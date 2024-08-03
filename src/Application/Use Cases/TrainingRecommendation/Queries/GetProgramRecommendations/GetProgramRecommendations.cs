using System.Text.Json.Serialization;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FitLog.Application.Use_Cases.CoachProfiles.Queries.GetCoachProfileDetails;

namespace FitLog.Application.TrainingRecommendation.Queries.GetProgramRecommendations
{
    public record GetProgramRecommendationsQuery : IRequest<Dictionary<string, List<ProgramOverviewDto>>>
    {
        [JsonIgnore]
        public string UserId { get; init; } = string.Empty;
    }

    public class GetProgramRecommendationsQueryValidator : AbstractValidator<GetProgramRecommendationsQuery>
    {
        public GetProgramRecommendationsQueryValidator()
        {
            RuleFor(v => v.UserId).NotEmpty();
        }
    }

    public class GetProgramRecommendationsQueryHandler : IRequestHandler<GetProgramRecommendationsQuery, Dictionary<string, List<ProgramOverviewDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetProgramRecommendationsQueryHandler(IApplicationDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, List<ProgramOverviewDto>>> Handle(GetProgramRecommendationsQuery request, CancellationToken cancellationToken)
        {
            // Get survey answers
            var surveyAnswerQuery = new GetUserTrainingSurveyQuery { UserId = request.UserId };
            var surveyAnswer = (SurveyAnswer)await _mediator.Send(surveyAnswerQuery, cancellationToken);

            // Retrieve all public programs
            var allPrograms = await _context.Programs
                .Include(p => p.User)
                .Where(p => p.PublicProgram == true)
                .ToListAsync();

            // Initialize dictionary to hold program recommendations
            var recommendations = new Dictionary<string, List<ProgramOverviewDto>>
            {
                { "DaysPerWeek", new List<ProgramOverviewDto>() },
                { "Goal", new List<ProgramOverviewDto>() },
                { "ExperienceLevel", new List<ProgramOverviewDto>() },
                { "GymType", new List<ProgramOverviewDto>() },
                { "AllCriteria", new List<ProgramOverviewDto>() }
            };

            foreach (var program in allPrograms)
            {
                var matchesDaysPerWeek = program.DaysPerWeek <= surveyAnswer.DaysPerWeek;
                var matchesGoal = HasSimilarGoal(program.Goal ?? "", surveyAnswer.Goal ?? "");
                var matchesExperienceLevel = HasSimilarExperienceLevel(program.ExperienceLevel ?? "", surveyAnswer.ExperienceLevel ?? "");
                var matchesGymType = HasSuitableGymType(program.GymType ?? "", surveyAnswer.GymType ?? "");

                var programDto = _mapper.Map<ProgramOverviewDto>(program);

                if (matchesDaysPerWeek) recommendations["DaysPerWeek"].Add(programDto);
                if (matchesGoal) recommendations["Goal"].Add(programDto);
                if (matchesExperienceLevel) recommendations["ExperienceLevel"].Add(programDto);
                if (matchesGymType) recommendations["GymType"].Add(programDto);

                if (matchesDaysPerWeek && matchesGoal && matchesExperienceLevel && matchesGymType)
                {
                    recommendations["AllCriteria"].Add(programDto);
                }
            }

            return recommendations;
        }

        private bool HasSimilarGoal(string programGoal, string userGoal)
        {
            List<string> programGoals = programGoal.Split(',').ToList();
            List<string> userGoals = userGoal.Split(',').ToList();

            return programGoals.Intersect(userGoals).Any(goal => ProgramAttributes.Goals.Contains(goal));
        }

        private bool HasSimilarExperienceLevel(string programExperienceLevel, string userExperienceLevel)
        {
            List<string> programSuitability = programExperienceLevel.Split(',')
                .Select(item => item.Trim().ToUpper())
                .ToList();

            return programSuitability.Contains(userExperienceLevel.ToUpper());
        }

        private bool HasSuitableGymType(string programGymType, string userGymType)
        {
            List<string> programGymTypes = programGymType.Split(',')
                .Select(item => item.Trim().ToUpper())
                .ToList();

            return programGymTypes.Contains(userGymType.ToUpper());
        }
    }
}
