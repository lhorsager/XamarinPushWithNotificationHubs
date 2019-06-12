using PushApiService.Enums;
using System;

namespace PushApiService.Dto
{
	public class DeviceInformation
	{
		public Guid DeviceId { get; set; }
		public Os Os { get; set; }
		public string OsVersion { get; set; }
		public string Manufacturer { get; set; }
		public string Model { get; set; }
		public string AppVersion { get; set; }
		public string PushToken { get; set; }
	}
}
