using PushApiService.Dto;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PushTestApp.Services
{
	public class ApiInstance
	{
		private static HttpClient _httpClient;
		private static HttpClient GetHttpClient()
		{
			if (_httpClient == null)
			{
				_httpClient = new HttpClient()
				{
					Timeout = new TimeSpan(0, 0, 5),
					BaseAddress = new Uri("https://mnemtest.azurewebsites.net/"),
				};
			}

			return _httpClient;
		}

		//public async Task<ServiceResponseBase> RegisterDevice(Guid deviceId, Os os, string osVersion, string manufacturer, string model, string appVersion, string pushToken)
		//{
		//	DeviceInformation deviceInformation = new DeviceInformation
		//	{
		//		DeviceId = deviceId,
		//		Os = os,
		//		OsVersion = osVersion,
		//		Manufacturer = manufacturer,
		//		Model = model,
		//		AppVersion = appVersion,
		//		PushToken = pushToken
		//	};

		//	HttpClient httpClient = GetHttpClient();
		//	Uri uri = new Uri(ApiInstance.GetCurrentApi() + "api/push/registerDevice");
		//	PolicyResult<HttpResponseMessage> response = null;
		//	ServiceResponseBase serviceResponseBase = null;

		//	try
		//	{
		//		string jsonRequestBody = JsonConvert.SerializeObject(deviceInformation);
		//		StringContent content = new StringContent(jsonRequestBody);
		//		content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

		//		response = await Policy
		//			.Handle<HttpRequestException>()
		//			.WaitAndRetryAsync(retryCount: 3,
		//							   sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(5))
		//			.ExecuteAndCaptureAsync(async () => await httpClient.PostAsync(uri, content));

		//		if (response.Result != null && response.Result.IsSuccessStatusCode)
		//		{
		//			string json = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
		//			serviceResponseBase = new ServiceResponseBase
		//			{
		//				IsNetworkConnected = IsNetworkConnected,
		//				ErrorMessage = "",
		//				IsSuccess = true
		//			};
		//		}
		//		else
		//		{
		//			System.Diagnostics.Debug.WriteLine($"RegisterDevice Error");

		//			serviceResponseBase = new ServiceResponseBase
		//			{
		//				IsNetworkConnected = IsNetworkConnected,
		//				ErrorMessage = "",
		//				IsSuccess = false
		//			};
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		System.Diagnostics.Debug.WriteLine($"RegisterDevice Exception: { ex }");

		//		serviceResponseBase = new ServiceResponseBase
		//		{
		//			IsNetworkConnected = IsNetworkConnected,
		//			ErrorMessage = "",
		//			IsSuccess = false
		//		};
		//	}

		//	return serviceResponseBase;
		//}
	}
}
