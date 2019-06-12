using PushApiService.Dto;
using System;

namespace PushApiService.Interfaces
{
	public interface IAuthenticationRepository
	{
		UserProfile CreateAccountFromEmail(EmailAccountRequest request);
		UserProfile Authenticate(AuthenticateRequest request);
		bool Validate(Guid token);
	}
}
