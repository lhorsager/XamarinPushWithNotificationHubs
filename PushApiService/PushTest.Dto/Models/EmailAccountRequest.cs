using PushApiService.Enums;
using System;

namespace PushApiService.Dto
{
	public class EmailAccountRequest
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DisplayName { get; set; }
		public string Password { get; set; }
		public Guid DeviceId { get; set; }
		public Os DeviceOs { get; set; }
		public string OsVersion { get; set; }
		public string DeviceManufacturer { get; set; }
		public string DeviceModel { get; set; }
		public string AppVersion { get; set; }
	}
}
