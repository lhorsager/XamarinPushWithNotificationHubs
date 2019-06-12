using System;

namespace PushApiService.Dto
{
	public class UserProfile
	{
		public Guid Token { get; set; }
		public string DisplayName { get; set; }
	}
}
