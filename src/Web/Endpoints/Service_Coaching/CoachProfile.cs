
using FitLog.Application.CoachProfiles.Commands.UpdateCoachApplicationStatus;
using FitLog.Application.CoachProfiles.Commands.UpdateCoachProfile;
using FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;
using FitLog.Application.CoachProfiles.Queries.GetCoachApplicationsWithPagination;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Users.Queries.ExternalLogin;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Coaching;

public class CoachProfile : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .MapGet(GetCoachProfileDetails, "{id}")
           .MapPost(UpdateCoachProfileDetails, "{id}")
           .MapPost(CreateCoachApplication, "apply-coach")
           .MapPut(UpdateCoachApplication, "update-application")
           .MapGet(GetApplicationsWithPagination,"paginated-list");
    }
    public async Task<bool> CreateCoachApplication(ISender sender, [FromBody] CreateCoachApplicationQuery request)
    {
        return await sender.Send(request);
    }
    public async Task<object> GetCoachProfileDetails(ISender sender,string id)
    {
        var request = new GetCoachProfileDetailsQuery(id);
        return await sender.Send(request);
    }
    public async Task<object> UpdateCoachProfileDetails(ISender sender,string id, [FromBody] UpdateCoachProfileCommand request)
    {
        return await sender.Send(request);
    }
    public async Task<object> UpdateCoachApplication(ISender sender, string id, [FromBody] UpdateCoachApplicationStatusCommand request)
    {
        return await sender.Send(request);
    }

    public async Task<object> GetApplicationsWithPagination(ISender sender,[AsParameters] GetCoachApplicationsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }
}
