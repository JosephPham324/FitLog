using FitLog.Application.CoachingServices.Commands.BookService;
using FitLog.Application.CoachingServices.Commands.CreateCoachingService;
using FitLog.Application.CoachingServices.Commands.DeleteCoachingService;
using FitLog.Application.CoachingServices.Commands.UpdateBookingStatus;
using FitLog.Application.CoachingServices.Commands.UpdateCoachingService;
using FitLog.Application.CoachingServices.Queries.GetBookingDetails;
using FitLog.Application.CoachingServices.Queries.GetCoachingServiceDetails;
using FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceList;
using FitLog.Application.CoachingServices.Queries.GetPaginatedCoachingServiceListOfUser;
using FitLog.Application.CoachingServices.Queries.GetServiceBookings;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_Coaching;

public class CoachingServices : EndpointGroupBase
{
    private readonly IUserTokenService _tokenService;
    private readonly IUser _identityService;

    public CoachingServices()
    {
        _tokenService = new CurrentUserFromToken(httpContextAccessor: new HttpContextAccessor());
        _identityService = new CurrentUser(httpContextAccessor: new HttpContextAccessor());
    }
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetCoachingServiceDetails, "{id}")
            .MapGet(GetPaginatedCoachingServices)
            .MapGet(GetPaginatedCoachingServicesOfUser, "user/{userId}")
            .MapPost(CreateCoachingService)
            .MapPut(UpdateCoachingService, "{id}")
            .MapDelete(DeleteCoachingService, "{id}")
            .MapPut(UpdateBookingStatus, "booking/{id}/status")
            .MapPost(BookService, "/book/{id}")
            .MapGet(GetBookingDetails, "booking/{id}") // Endpoint for getting booking details
            .MapGet(GetServiceBookings, "service/{serviceId}/bookings"); // New endpoint for getting service bookings
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


    //[Microsoft.AspNetCore.Authorization.Authorize("CoachOnly")]
    public Task<Result> CreateCoachingService(ISender sender, [FromBody] CreateCoachingServiceCommand command)
    {
        return sender.Send(command);
    }

    //[Microsoft.AspNetCore.Authorization.Authorize("CoachOnly")]
    public async Task<Result> UpdateCoachingService(ISender sender, int id, [FromBody] UpdateCoachingServiceCommand command)
    {
        if (id != command.Id) return Result.Failure(["Id doesn't match instance"]);
        var result = await sender.Send(command);
        return result;
    }

    //[Microsoft.AspNetCore.Authorization.Authorize("CoachOnly")]
    public async Task<Result> DeleteCoachingService(ISender sender, int id)
    {
        var result = await sender.Send(new DeleteCoachingServiceCommand { Id = id });
        return result;
    }

    public async Task<Result> UpdateBookingStatus(ISender sender, int id, [FromBody] UpdateBookingStatusCommand command)
    {
        if (id != command.BookingId) return Result.Failure(new[] { "Booking ID doesn't match instance" });
        var result = await sender.Send(command);
        return result;
    }

    //[Microsoft.AspNetCore.Authorization.Authorize("MemberOnly")]
    public async Task<Result> BookService(ISender sender, int id, [FromBody] BookServiceCommand command)
    {
        if (id != command.CoachingServiceId)
        {
            return Result.Failure(["Service id mismatch"]);

        }
        command.UserId = _identityService.Id ?? "";

        var result = await sender.Send(command);
        return result;
    }

    //[Microsoft.AspNetCore.Authorization.Authorize("MemberOnly")]
    public async Task<object> GetBookingDetails(ISender sender, int id)
    {
        var query = new GetBookingDetailsQuery(id);
        return await sender.Send(query);
    }

    //[Microsoft.AspNetCore.Authorization.Authorize("CoachOnly")]
    public async Task<object> GetServiceBookings(ISender sender, int serviceId)
    {
        var query = new GetServiceBookingsQuery(serviceId);
        return await sender.Send(query);
    }
}
