using PushApiService.Dto;

namespace PushTestApp.Services
{
	public class CreateAccountAuthResponse : ServiceResponseBase
	{
		public UserProfile UserProfile { get; set; }
	}
}
