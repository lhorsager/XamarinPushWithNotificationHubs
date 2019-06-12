using Microsoft.AspNetCore.Mvc;
using PushApiService.Dto;
using PushApiService.Interfaces;
using System;

namespace PushApiService.Controllers
{
	[ApiController]
    public class AuthenticationController : ControllerBase
    {
		private readonly IAuthenticationRepository _repo;

		public AuthenticationController(IAuthenticationRepository repo)
		{
			_repo = repo;
		}

		[HttpPost]
		[Route("api/authentication/CreateAccountFromEmail")]
		public ActionResult<UserProfile> CreateAccountFromEmail([FromBody] EmailAccountRequest request)
		{
			try
			{
				return _repo.CreateAccountFromEmail(request);
			}
			catch (Exception ex)
			{
				return BadRequest("There was an error creating this account. " + ex.Message);
			}
			
		}

		[HttpPost]
		[Route("api/authentication/authenticate")]
		public ActionResult<UserProfile> Authenticate([FromBody] AuthenticateRequest request)
		{
			try
			{
				return _repo.Authenticate(request);
			}
			catch (UnauthorizedAccessException)
			{
				return Unauthorized();
			}
			catch (Exception)
			{
				return BadRequest("There was an error creating this account.");
			}
		}

		[HttpPost]
		[Route("api/authentication/validate")]
		public ActionResult<bool> Validate([FromBody] Guid token)
		{
			return _repo.Validate(token);
		}
	}
}