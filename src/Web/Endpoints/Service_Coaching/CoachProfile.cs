
using FitLog.Application.Certifications.Commands;
using FitLog.Application.Certifications.Commands.CreateCertification;
using FitLog.Application.Certifications.Commands.DeleteCertification;
using FitLog.Application.Certifications.Commands.UpdateCertification;
using FitLog.Application.Certifications.Queries;
using FitLog.Application.Certifications.Queries.GetCertificationById;
using FitLog.Application.Certifications.Queries.GetCertificationsByUserId;
using FitLog.Application.CoachProfiles.Commands.UpdateCoachApplicationStatus;
using FitLog.Application.CoachProfiles.Commands.UpdateCoachProfile;
using FitLog.Application.CoachProfiles.Queries.CreateCoachApplication;
using FitLog.Application.CoachProfiles.Queries.GetCoachApplicationsWithPagination;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.ExternalLogin;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Coaching;

public class CoachProfile : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public CoachProfile()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
          .MapGet(GetCertificationById, "{userId}/certifications/{id}")
          .MapGet(GetCoachProfileDetails, "{id}")
          .MapGet(GetCertificationsByUserId, "certifications/user/{userId}");

        app.MapGroup(this)
           //.RequireAuthorization()
           .MapPost(UpdateCoachProfileDetails, "{id}")
           .MapPost(CreateCoachApplication, "apply-coach")
           .MapPut(UpdateCoachApplication, "update-application")
           // Certification Endpoints
           .MapPost(CreateCertification, "certifications")
           .MapPut(UpdateCertification, "certifications/{id}")
           .MapDelete(DeleteCertification, "certifications/{id}");

        app.MapGroup(this)
           //.RequireAuthorization("AdminOnly")
           .MapGet(GetApplicationsWithPagination, "paginated-list");
    }
    #region Gets
    public async Task<CertificationDTO> GetCertificationById(ISender sender, [FromRoute] string userId, [FromRoute] int id)
    {
        var query = new GetCertificationByIdQuery() { CertificationId = id };
        return await sender.Send(query);
    }

    public async Task<List<CertificationDTO>> GetCertificationsByUserId(ISender sender, string userId)
    {
        var query = new GetCertificationsByUserIdQuery()
        {
            UserId = userId
        };
        return await sender.Send(query);
    }
    public async Task<object> GetCoachProfileDetails(ISender sender, string id)
    {
        var request = new GetCoachProfileDetailsQuery(id);
        return await sender.Send(request);
    }


    public async Task<PaginatedList<CoachApplicationDto>> GetApplicationsWithPagination(ISender sender, [AsParameters] GetCoachApplicationsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    #endregion

    public async Task<Result> CreateCoachApplication(ISender sender, [FromBody] CreateCoachApplicationQuery request)
    {
        return await sender.Send(request);
    }
   
    public async Task<Result> UpdateCoachProfileDetails(ISender sender,string id, [FromBody] UpdateCoachProfileCommand request)
    {
        return await sender.Send(request);
    }
    public async Task<Result> UpdateCoachApplication(ISender sender, string id, [FromBody] UpdateCoachApplicationStatusCommand request)
    {
        return await sender.Send(request);
    }

    public async Task<Result> CreateCertification(ISender sender, [FromBody] CreateCertificationCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<Result> UpdateCertification(ISender sender, int id, [FromBody] UpdateCertificationCommand command)
    {
        if (id != command.CertificationId) return Result.Failure(new[] { "Certification ID doesn't match instance" });
        return await sender.Send(command);
    }

    public async Task<Result> DeleteCertification(ISender sender, int id)
    {
        var command = new DeleteCertificationCommand()
        {
            CertificationId = id
        };
        return await sender.Send(command);
    }

   
}
