using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingSurvey : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public TrainingSurvey()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateTrainingSurvey, "create")
            .MapPut(UpdateTrainingSurvey, "update/{id}")
            .MapGet(GetUserTrainingSurveyAnswer, "user");
    }

    public Task<Result> CreateTrainingSurvey(ISender sender, [FromBody] CreateSurveyAnswerCommand command)
    {
        command.UserId = _identityService.Id ?? "";

        return sender.Send(command);
    }

    public Task<Result> UpdateTrainingSurvey(ISender sender,int id, [FromBody] UpdateTrainingSurveyAnswersCommand command)
    {
        command.UserId = _identityService.Id ?? "";

        if (id != command.SurveyAnswerId)
        {
            return Task.FromResult(Result.Failure(["Id mismatch"]));
        }
        return sender.Send(command);
    }

    public Task<SurveyAnswer> GetUserTrainingSurveyAnswer(ISender sender, [AsParameters] GetUserTrainingSurveyQuery query)
    {
        return sender.Send(query);
    }
}
