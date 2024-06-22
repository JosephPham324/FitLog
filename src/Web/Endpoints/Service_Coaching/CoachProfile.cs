
using FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;
using FitLog.Application.Users.Queries.ExternalLogin;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Coaching;

public class CoachProfile : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
                   .MapPost(CreateCoachApplication, "apply-coach");
    }
    public async Task<bool> CreateCoachApplication(ISender sender, [FromBody] CreateCoachApplicationQuery request)
    {
        return await sender.Send(request);
    }
}
