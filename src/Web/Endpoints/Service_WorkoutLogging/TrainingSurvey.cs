using FitLog.Application.Common.Models;
using FitLog.Application.TrainingSurvey.Queries.GetUserTrainingSurvey;
using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Application.Users.Queries.GetUsers;
using FitLog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingSurvey : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateTrainingSurvey, "create")
            .MapPut(UpdateTrainingSurvey, "update/{id}")
            .MapGet(GetUserTrainingSurveyAnswer, "user");
    }

    public Task<Result> CreateTrainingSurvey(ISender sender, [FromBody] CreateSurveyAnswerCommand command)
    {
        return sender.Send(command);
    }

    public Task<Result> UpdateTrainingSurvey(ISender sender,int id, [FromBody] UpdateTrainingSurveyAnswersCommand command)
    {
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
