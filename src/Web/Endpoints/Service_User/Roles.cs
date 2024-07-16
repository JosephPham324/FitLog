using FitLog.Application.Common.Models;
using FitLog.Application.Roles.Commands.AddRole;
using FitLog.Application.Roles.Commands.DeleteRole;
using FitLog.Application.Roles.Commands.UpdateRole;
using FitLog.Application.Roles.Queries.GetAllRoles;
using FitLog.Application.Roles.Queries.GetRoleById;
using Microsoft.AspNetCore.Mvc;

namespace FitLog.Web.Endpoints.Service_User;

public class Roles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetRolesList)
            .MapGet(GetRoleById, "{id}")
            .MapPost(CreateRole)
            .MapPut(UpdateRole, "{id}")
            .MapDelete(DeleteRole, "{id}");
    }

    #region Queries
    public Task<List<RoleDto>> GetRolesList(ISender sender, [AsParameters] GetAllRolesQuery query)
    {
        return sender.Send(query);
    }

    public Task<RoleDto> GetRoleById(ISender sender, string id)
    {
        return sender.Send(new GetRoleByIdQuery { RoleId = id });
    }
    #endregion

    #region Commands
    //Create
    public Task<Result> CreateRole(ISender sender, [FromBody] AddRoleCommand command)
    {
        return sender.Send(command);
    }
    //Update
    public Task<Result> UpdateRole(ISender sender, int id, [FromBody] UpdateRoleCommand command)
    {
        //Check matching id
        if (id != command.RoleId) return Task.FromResult(Result.Failure(new List<string> { "Id mismatch" }));
        return sender.Send(command);
    }
    //Delete
    public Task<Result> DeleteRole(ISender sender,int id, [FromBody] DeleteRoleCommand command)
    {
        //Check matching id
        if (id != command.RoleId) return Task.FromResult(Result.Failure(new List<string> { "Id mismatch" }));
        return sender.Send(command);
    }
    #endregion


}
