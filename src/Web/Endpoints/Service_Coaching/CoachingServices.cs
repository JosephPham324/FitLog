using FitLog.Application.CoachingServices.Commands.CreateCoachingService;
using FitLog.Application.CoachingServices.Commands.DeleteCoachingService;
using FitLog.Application.CoachingServices.Commands.UpdateCoachingService;
using FitLog.Application.CoachingServices.Queries.GetCoachingServiceDetails;
using FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceList;
using FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceListOfUser;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUserDetails;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Coaching;

public class CoachingServices : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapGet(GetCoachingServiceDetails, "{id}")
            .MapGet(GetPaginatedCoachingServices)
            .MapGet(GetPaginatedCoachingServicesOfUser, "user/{userId}")
            .MapPost(CreateCoachingService)
            .MapPut(UpdateCoachingService, "{id}")
            .MapDelete(DeleteCoachingService, "{id}");
    }

    public Task<CoachingServiceDetailsDto> GetCoachingServiceDetails(ISender sender, int id)
    {
        return sender.Send(new GetCoachingServiceDetailsQuery { Id = id });
    }

    public Task<PaginatedList<CoachingServiceDTO>> GetPaginatedCoachingServices(ISender sender, [AsParameters] GetPaginatedCoachingServiceListQuery query)
    {
        return sender.Send(query);
    }

    public Task<PaginatedList<CoachingServiceDTO>> GetPaginatedCoachingServicesOfUser(ISender sender, string userId, [AsParameters] GetPaginatedCoachingServiceListOfUserQuery query)
    {
        query = query with { UserId = userId };
        return sender.Send(query);
    }

    public Task<int> CreateCoachingService(ISender sender, [FromBody] CreateCoachingServiceCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateCoachingService(ISender sender, int id, [FromBody] UpdateCoachingServiceCommand command)
    {
        if (id != command.Id) return Results.BadRequest("None matching id between request and data");
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteCoachingService(ISender sender, int id)
    {
        await sender.Send(new DeleteCoachingServiceCommand { Id = id });
        return Results.NoContent();
    }
}
