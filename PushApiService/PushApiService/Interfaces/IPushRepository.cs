using PushApiService.Dto;
using PushApiService.Enums;
using System;
using System.Threading.Tasks;

namespace PushApiService.Interfaces
{
	public interface IPushRepository
	{
		bool ValidateToken(string token);
		void RegisterDevice(DeviceInformation Request);
		void UpdateDevice(DeviceInformation Request);
		Task SendPush(PushSend Request);
		Task SendByDevices(string messageType, Guid objectId, string[] devices, Os Os);
	}
}
