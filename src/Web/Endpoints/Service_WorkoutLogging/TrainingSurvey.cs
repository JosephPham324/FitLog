using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Application.Users.Queries.GetUsers;
using Microsoft.Win32;

namespace FitLog.Web.Endpoints.Service_WorkoutLogging;

public class TrainingSurvey : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateTrainingSurvey, "create");
    }

    public Task<TrainingSurveyDTO> CreateTrainingSurvey(ISender sender, [AsParameters] CreateSurveyAnswerCommand command)
    {
        return sender.Send(command);
    }
}
