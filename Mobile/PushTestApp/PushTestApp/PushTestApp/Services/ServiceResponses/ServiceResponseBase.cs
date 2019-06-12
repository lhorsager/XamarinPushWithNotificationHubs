namespace PushTestApp.Services
{
	public class ServiceResponseBase
	{
		public bool IsNetworkConnected { get; set; }
		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; }
	}
}
