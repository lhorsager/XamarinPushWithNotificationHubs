using Newtonsoft.Json;
using Polly;
using PushApiService.Dto;
using PushApiService.Enums;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PushTestApp.Services
{
	public class ApiInstance
	{
		private Guid _deviceId;
		private Guid? _token;

		const string baseAddress = "https://mnemtest.azurewebsites.net/";

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

		public ApiInstance()
		{
			//We need a device id if we don't have one
			string strDeviceId = Preferences.Get("DeviceId", "");
			if(String.IsNullOrEmpty(strDeviceId))
			{
				_deviceId = Guid.NewGuid();
				Preferences.Set("DeviceId", _deviceId.ToString());
			}
			else
			{
				_deviceId = Guid.Parse(strDeviceId);
			}

			string strToken = Preferences.Get("Token", "");
			if(!String.IsNullOrEmpty(strToken))
			{
				_token = Guid.Parse(strToken);
			}
		}

		public bool IsAuthenticated()
		{
			return _token.HasValue;
		}

		public async Task<ServiceResponseBase> RegisterDevice()
		{
			DeviceInformation deviceInformation = new DeviceInformation
			{
				DeviceId = _deviceId,
				OsVersion = DeviceInfo.VersionString,
				Manufacturer = DeviceInfo.Manufacturer,
				Model = DeviceInfo.Model,
				AppVersion = VersionTracking.CurrentVersion
			};

			switch (DeviceInfo.Platform.ToString())
			{
				case "iOS":
					deviceInformation.Os = Os.iOS;
					break;
				case "Android":
					deviceInformation.Os = Os.Android;
					break;
				default:
					break;
			}

			HttpClient httpClient = GetHttpClient();
			Uri uri = new Uri(baseAddress + "api/push/registerDevice");
			PolicyResult<HttpResponseMessage> response = null;
			ServiceResponseBase serviceResponseBase = null;

			try
			{
				string jsonRequestBody = JsonConvert.SerializeObject(deviceInformation);
				StringContent content = new StringContent(jsonRequestBody);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				response = await Policy
					.Handle<HttpRequestException>()
					.WaitAndRetryAsync(retryCount: 3,
									   sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(5))
					.ExecuteAndCaptureAsync(async () => await httpClient.PostAsync(uri, content));

				if (response.Result != null && response.Result.IsSuccessStatusCode)
				{
					string json = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
					serviceResponseBase = new ServiceResponseBase
					{
						ErrorMessage = "",
						IsSuccess = true
					};
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"RegisterDevice Error");

					serviceResponseBase = new ServiceResponseBase
					{
						ErrorMessage = "",
						IsSuccess = false
					};
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"RegisterDevice Exception: { ex }");

				serviceResponseBase = new ServiceResponseBase
				{
					ErrorMessage = "",
					IsSuccess = false
				};
			}

			return serviceResponseBase;
		}

		public async Task<CreateAccountAuthResponse> CreateAccountFromEmail(string email, string firstName, string lastName, string displayName, string password)
		{
			EmailAccountRequest emailAccountRequest = new EmailAccountRequest
			{
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				DisplayName = displayName,
				Password = password,
				DeviceId = _deviceId,
				OsVersion = DeviceInfo.VersionString,
				DeviceManufacturer = DeviceInfo.Manufacturer,
				DeviceModel = DeviceInfo.Model,
				AppVersion = VersionTracking.CurrentVersion
			};

			switch (DeviceInfo.Platform.ToString())
			{
				case "iOS":
					emailAccountRequest.DeviceOs = Os.iOS;
					break;
				case "Android":
					emailAccountRequest.DeviceOs = Os.Android;
					break;
				default:
					break;
			}

			HttpClient httpClient = GetHttpClient();
			Uri uri = new Uri(baseAddress + "api/authentication/CreateAccountFromEmail");
			PolicyResult<HttpResponseMessage> response = null;
			CreateAccountAuthResponse createAccountAuthResponse = null;
			UserProfile userProfile = null;
			string responseErrorMessage = null;

			try
			{
				string jsonRequestBody = JsonConvert.SerializeObject(emailAccountRequest);
				StringContent content = new StringContent(jsonRequestBody);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				response = await Policy
					.Handle<HttpRequestException>()
					.WaitAndRetryAsync(retryCount: 3,
									   sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(5))
					.ExecuteAndCaptureAsync(async () => await httpClient.PostAsync(uri, content));

				if (response.Result != null && response.Result.IsSuccessStatusCode)
				{
					string json = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
					userProfile = await Task.Run(() => JsonConvert.DeserializeObject<UserProfile>(json)).ConfigureAwait(false);

					createAccountAuthResponse = new CreateAccountAuthResponse
					{
						IsSuccess = true,
						ErrorMessage = "",
						UserProfile = userProfile
					};
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"CreateAccountFromEmail Error");

					responseErrorMessage = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);

					createAccountAuthResponse = new CreateAccountAuthResponse
					{
						IsSuccess = false,
						ErrorMessage = responseErrorMessage,
						UserProfile = null
					};
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"CreateAccountFromEmail Exception: { ex }");

				responseErrorMessage = "An error has occurred.";

				createAccountAuthResponse = new CreateAccountAuthResponse
				{
					IsSuccess = false,
					ErrorMessage = responseErrorMessage,
					UserProfile = null
				};
			}

			Preferences.Set("Token", createAccountAuthResponse.UserProfile.Token.ToString());

			return createAccountAuthResponse;
		}

		public async Task<CreateAccountAuthResponse> Authenticate(string email, string password)
		{
			AuthenticateRequest authenticateRequest = new AuthenticateRequest
			{
				Email = email,
				Password = password,
				DeviceId = _deviceId,
				OsVersion = DeviceInfo.VersionString,
				DeviceManufacturer = DeviceInfo.Manufacturer,
				DeviceModel = DeviceInfo.Model,
				AppVersion = VersionTracking.CurrentVersion
			};

			switch (DeviceInfo.Platform.ToString())
			{
				case "iOS":
					authenticateRequest.DeviceOs = Os.iOS;
					break;
				case "Android":
					authenticateRequest.DeviceOs = Os.Android;
					break;
				default:
					break;
			}

			HttpClient httpClient = GetHttpClient();
			Uri uri = new Uri(baseAddress + "api/authentication/authenticate");
			PolicyResult<HttpResponseMessage> response = null;
			UserProfile userProfile = null;
			CreateAccountAuthResponse createAccountAuthResponse = null;

			try
			{
				string jsonRequestBody = JsonConvert.SerializeObject(authenticateRequest);
				StringContent content = new StringContent(jsonRequestBody);
				content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

				response = await Policy
					.Handle<HttpRequestException>()
					.WaitAndRetryAsync(retryCount: 3,
									   sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(5))
					.ExecuteAndCaptureAsync(async () => await httpClient.PostAsync(uri, content));

				if (response.Result != null && response.Result.IsSuccessStatusCode)
				{
					string json = await response.Result.Content.ReadAsStringAsync().ConfigureAwait(false);
					userProfile = await Task.Run(() => JsonConvert.DeserializeObject<UserProfile>(json)).ConfigureAwait(false);

					if(userProfile != null)
					{
						Preferences.Set("Token", userProfile.Token.ToString());

						createAccountAuthResponse = new CreateAccountAuthResponse
						{
							IsSuccess = true,
							ErrorMessage = "",
							UserProfile = userProfile
						};
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"Authenticate Error");

					string errorMessage = string.Empty;
					if (response.Result != null && response.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						errorMessage = "The email or password you entered is incorrect. Please retry.";
					}

					createAccountAuthResponse = new CreateAccountAuthResponse
					{
						IsSuccess = false,
						ErrorMessage = errorMessage,
						UserProfile = null
					};
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Authenticate Exception: { ex }");

				createAccountAuthResponse = new CreateAccountAuthResponse
				{
					IsSuccess = false,
					ErrorMessage = "",
					UserProfile = null
				};
			}

			Preferences.Set("Token", createAccountAuthResponse.UserProfile.Token.ToString());

			return createAccountAuthResponse;
		}
	}
}
