using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Configuration;
using PushApiService.Data;
using PushApiService.Dto;
using PushApiService.Enums;
using PushApiService.Interfaces;
using PushApiService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PushApiService.Repositories
{
	public class PushRepository : IPushRepository
	{
		#region Fields
		private readonly PushDataContext _db;
		private readonly IConfiguration _config;
		private Guid _token;
		private Guid _userId;
		#endregion

		#region Constructors
		public PushRepository(IConfiguration config, PushDataContext db)
		{
			_db = db;
			_config = config;
		}
		#endregion

		#region Methods
		public bool ValidateToken(string token)
		{
			if (String.IsNullOrEmpty(token))
				return false;

			string parsedToken = token.Replace("Bearer", "").Replace("bearer", "").Replace(" ", "");
			if (String.IsNullOrEmpty(parsedToken))
				return false;

			Guid _bearerToken = Guid.Parse(parsedToken);
			var _tokens = from t in _db.Tokens
						  where t.Id == _bearerToken
						  select t;

			if (_tokens.Count() > 0)
			{
				_token = _bearerToken;
				_userId = _tokens.First().UserId;
				return true;
			}
			else
				return false;
		}

		public void RegisterDevice(DeviceInformation request)
		{
			//First create or find device
			Device _device = _db.Devices.Where(d => d.Id == request.DeviceId).FirstOrDefault();
			if (_device == null)
			{
				_device = new Device
				{
					Id = request.DeviceId,
					AppVersion = request.AppVersion,
					CreatedOn = DateTime.UtcNow,
					LastUpdated = DateTime.UtcNow,
					Manufacturer = request.Manufacturer,
					Model = request.Model,
					Os = request.Os,
					OsVersion = request.OsVersion
				};

				_db.Devices.Add(_device);
			}
			else
			{
				_device.AppVersion = request.AppVersion;
				_device.LastUpdated = DateTime.UtcNow;
				_device.Manufacturer = request.Manufacturer;
				_device.Model = request.Model;
				_device.Os = request.Os;
				_device.OsVersion = request.OsVersion;
			}
			_db.SaveChanges();
		}

		public void UpdateDevice(DeviceInformation request)
		{
			//First create or find device
			Device _device = _db.Devices.Where(d => d.Id == request.DeviceId).FirstOrDefault();
			if (_device == null)
			{
				_device = new Device
				{
					Id = request.DeviceId,
					AppVersion = request.AppVersion,
					CreatedOn = DateTime.UtcNow,
					LastUpdated = DateTime.UtcNow,
					Manufacturer = request.Manufacturer,
					Model = request.Model,
					Os = request.Os,
					OsVersion = request.OsVersion
				};

				_db.Devices.Add(_device);
			}
			else
			{
				_device.AppVersion = request.AppVersion;
				_device.LastUpdated = DateTime.UtcNow;
				_device.Manufacturer = request.Manufacturer;
				_device.Model = request.Model;
				_device.Os = request.Os;
				_device.OsVersion = request.OsVersion;
			}
			_db.SaveChanges();
		}

		public async Task SendPush(PushSend Request)
		{
			string hubNotificationPath = _config.GetValue<string>("PushNotification:HubNotificationPath");
			string hubConnectionString = _config.GetValue<string>("PushNotification:HubConnectionString");
			NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(hubConnectionString, hubNotificationPath);

			//APPLE
			var _tokensiOS = (from t in _db.Tokens
						   from d in _db.Devices
						   where t.UserId == Request.UserId
								&& t.DeviceId == d.Id
								&& d.Os == Os.iOS
						   orderby d.LastUpdated descending
						   select t.DeviceId.ToString()).Take(20).Distinct().ToList();

			string msgiOS = @"{ ""aps"" : { ""content-available"" : 1 } , ""messageType"" : """ + Request.MessageType + @""" , ""objectId"" : """ + Request.ObjectId.ToString() + @""" }";


			//GOOGLE
			var _tokensAndroid = (from t in _db.Tokens
								  from d in _db.Devices
								  where t.UserId == Request.UserId
									   && t.DeviceId == d.Id
									   && d.Os == Os.Android
								  orderby d.LastUpdated descending
								  select t.DeviceId.ToString()).Take(20).Distinct().ToList();

			string msgAndroid = @"{ ""data"" : {  ""messageType"" : """ + Request.MessageType + @""" , ""objectId"" : """ + Request.ObjectId.ToString() + @"""  } }";


			//SENDS
			if(_tokensiOS.Count()>0)
				await hub.SendAppleNativeNotificationAsync(msgiOS, _tokensiOS);

			if(_tokensAndroid.Count()>0)
				await hub.SendFcmNativeNotificationAsync(msgAndroid, _tokensAndroid);

		}

		public async Task SendByDevices(string messageType, Guid objectId, string[] devices, Os Os)
		{
			NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://andelinprodhub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=CUSlt4IBNZKgp9QFOdc+of9HG6xWEGESFx85ze2PpLI=", "andelinprodnotification");

			if (devices.Count() > 0)
			{
				switch (Os)
				{
					case Os.iOS:
						string msgiOS = @"{ ""aps"" : { ""content-available"" : 1 } , ""messageType"" : """ + messageType + @""" , ""objectId"" : """ + objectId.ToString() + @""" }";
						await hub.SendAppleNativeNotificationAsync(msgiOS, devices);
						break;
					case Os.Android:
						string msgAndroid = @"{ ""data"" : {  ""messageType"" : """ + messageType + @""" , ""objectId"" : """ + objectId.ToString() + @"""  } }";
						await hub.SendFcmNativeNotificationAsync(msgAndroid, devices);
						break;
					case Os.Windows:
						break;
					case Os.Browser:
						break;
					default:
						break;
				}
			}			
		}
		#endregion
	}
}
