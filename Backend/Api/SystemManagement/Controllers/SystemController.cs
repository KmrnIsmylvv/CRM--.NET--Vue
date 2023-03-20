using Elfo.Round.Identity;
using Elfo.Round.Mvc;
using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands;
using Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Controllers
{
	[Route("api/system")]
	[DBAuthorize]
	public class SystemController : ControllerBase
	{
		readonly IMediator mediator;
        public SystemController(IMediator mediator) => this.mediator = mediator;

		#region User Queries

		[HttpGet("user")]
		public async Task<ActionResult<GetUser.Result>> GetUser([FromQuery]GetUser.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("users")]
		public async Task<ActionResult<ListResult<GetUsersList.Result>>> GetUsersList([FromBody]GetUsersList.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("userFullNames")]
		public async Task<ActionResult<List<GetUserFullNames.Result>>> GetUserFullNames([FromBody]GetUserFullNames.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("userFullNamesToImpersonate")]
		public async Task<ActionResult<List<GetUserFullNamesToImpersonate.Result>>> GetUserFullNamesToImpersonate([FromBody]GetUserFullNamesToImpersonate.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("getTeachers")]
		public async Task<ActionResult<Task<GetTeachers.Result>>> GetTeachers([FromBody] GetTeachers.Query query)
		{
            var result = await mediator.Send(query);
            return this.ResultOk(result);
        }

		#endregion

		#region User Commands

		[HttpPost("user/create")]
		public async Task<ActionResult<int>> CreateUser([FromBody]CreateUserCommand command)
		{
			var result = await mediator.Send(command);
			return this.ResultOk(result);
		}

		[HttpPost("user/update")]
		public async Task<ActionResult> UpdateUser([FromBody]UpdateUserCommand command, [FromServices]IIdentityService identityService)
		{
			await mediator.Send(command);
            identityService.RefreshUser(command.Username);
            return this.ResultOk();
		}

		[HttpPost("user/enable")]
		public async Task<ActionResult> EnableUser([FromQuery]EnableUserCommand command, [FromServices]IIdentityService identityService)
		{
			var usernameToReset = await mediator.Send(command);
			identityService.RefreshUser(usernameToReset);
			return this.ResultOk();
		}

		[HttpPost("user/disable")]
		public async Task<ActionResult> DisableUser([FromQuery]DisableUserCommand command, [FromServices]IIdentityService identityService)
		{
			var usernameToReset = await mediator.Send(command);
			identityService.RefreshUser(usernameToReset);
			return this.ResultOk();
		}

		#endregion

		#region Group Queries

		[HttpGet("group")]
		public async Task<ActionResult<GetGroup.Result>> GetGroup([FromQuery]GetGroup.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("groups")]
		public async Task<ActionResult<ListResult<GetGroupsList.Result>>> GetGroupsList([FromBody]GetGroupsList.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		[HttpPost("groupNames")]
		public async Task<ActionResult<List<GetGroupNames.Result>>> GetGroupNames([FromBody]GetGroupNames.Query query)
		{
			var result = await mediator.Send(query);
			return this.ResultOk(result);
		}

		#endregion

		#region Group Commands

		[HttpPost("group/create")]
		[ExceptionHandler(typeof(CreateGroupCommand))]
		public async Task<ActionResult<short>> CreateGroup([FromBody]CreateGroupCommand command)
		{
			var result = await mediator.Send(command);
			return this.ResultOk(result);
		}

		[HttpPost("group/update")]
		[ExceptionHandler(typeof(UpdateGroupCommand))]
		public async Task<ActionResult> UpdateGroup([FromBody]UpdateGroupCommand command, [FromServices]IUserCacheManager userCacheManager)
		{
			await mediator.Send(command);
			userCacheManager.Clear();
			return this.ResultOk();
		}

		[HttpPost("group/enable")]
		public async Task<ActionResult> EnableGroup([FromQuery]EnableGroupCommand command, [FromServices]IUserCacheManager userCacheManager)
		{
			await mediator.Send(command);
			userCacheManager.Clear();
			return this.ResultOk();
		}

		[HttpPost("group/disable")]
		public async Task<ActionResult> DisableGroup([FromQuery]DisableGroupCommand command, [FromServices]IUserCacheManager userCacheManager)
		{
			await mediator.Send(command);
			userCacheManager.Clear();
			return this.ResultOk();
		}

		#endregion

		#region Permissions

		[HttpGet("permissions")]
		public async Task<ActionResult<List<string>>> GetPermissions()
		{
			var result = await mediator.Send(new GetPermissionsQuery());
			return this.ResultOk(result);
		}

        #endregion

        #region Cache

        [HttpPut("clearUsersCache")]
		public async Task<ActionResult> ClearUsersCache()
		{
			await mediator.Send(new ClearUsersCacheCommand());
			return this.ResultOk();
		}

		[HttpPut("removeUeUserFromCache")]
		public async Task<ActionResult> RemoveUeUserFromCache()
		{
			await mediator.Send(new RemoveUeUserFromCacheCommand());
			return this.ResultOk();
		}

		#endregion

		#region Hangfire
		[HttpPost("set-auth-cookie")]
		[DBAuthorize(Permissions.HangfireDashboard_Reader)]
		public ActionResult SetCookie()
		{
			if (Request.Headers.ContainsKey("Authorization"))
			{
				var parts = Request.Headers["Authorization"].ToString().Split(" ");

				if (parts.Length == 2)
				{
					Response.Cookies.Append("access_token", parts[1], new CookieOptions
					{
						Domain = Request.Host.Host.ToString(),
						Secure = true,
						SameSite = SameSiteMode.None
					});
				}
			}

			return Ok();
		}

		[HttpPost("reset-auth-cookie")]
		[DBAuthorize(Permissions.HangfireDashboard_Reader)]
		public ActionResult ResetCookie()
		{
			Response.Cookies.Append("access_token", "", new CookieOptions
			{
				Domain = Request.Host.Host.ToString(),
				Expires = DateTimeOffset.MinValue
			});

			return Ok();
		}
		#endregion
	}
}
