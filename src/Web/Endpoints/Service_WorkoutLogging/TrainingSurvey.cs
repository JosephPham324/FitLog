using FitLog.Application.Common.Models;
using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingSurvey : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateTrainingSurvey, "create");
    }

    public Task<Result> CreateTrainingSurvey(ISender sender, [FromBody] CreateSurveyAnswerCommand command)
    {
        return sender.Send(command);
    }
}
