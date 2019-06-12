using System;

namespace PushApiService.Dto
{
	public class RegisterDeviceEventArgs : EventArgs
	{
		public string PushToken { get; private set; }
		public Guid DeviceId { get; private set; }

		public RegisterDeviceEventArgs(Guid deviceId, string pushToken)
		{
			PushToken = pushToken;
			DeviceId = deviceId;
		}
	}

	public delegate void RegisterDeviceHandler(object sender, RegisterDeviceEventArgs e);
}
