using PushApiService.Data;
using PushApiService.Dto;
using PushApiService.Interfaces;
using PushApiService.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PushApiService.Repositories
{
	public class AuthenticationRepository : IAuthenticationRepository
	{
		#region Fields
		private readonly PushDataContext _db;

		#endregion

		#region Constructors
		public AuthenticationRepository(PushDataContext db)
		{
			_db = db;
		}
		#endregion

		#region Methods
		public UserProfile CreateAccountFromEmail(EmailAccountRequest request)
		{
			//Check to make sure doesn't already exist
			var current = _db.Users.Where(e => e.Email == request.Email).Count();
			if(current>0)
			{
				throw new Exception();
			}

			//Map request to User object
			User user = new User()
			{
				Id = Guid.NewGuid(),
				CreatedOn = DateTime.UtcNow,
				DisplayName = request.DisplayName,
				Email = request.Email,
				LastLogin = null,
				PasswordHash = HashPassword(request.Password)
			};

			_db.Users.Add(user);

			//See if device exists
			Device device = _db.Devices.Where(d => d.Id == request.DeviceId).FirstOrDefault();
			if (device == null)
			{
				device = new Device
				{
					Id = request.DeviceId,
					AppVersion = request.AppVersion,
					CreatedOn = DateTime.UtcNow,
					LastUpdated = DateTime.UtcNow,
					Manufacturer = request.DeviceManufacturer,
					Model = request.DeviceModel,
					Os = request.DeviceOs,
					OsVersion = request.OsVersion
				};

				_db.Devices.Add(device);
			}

			//Create token
			Token token = new Token
			{
				Id = Guid.NewGuid(),
				CreatedOn = DateTime.UtcNow,
				DeviceId = device.Id,
				UserId = user.Id
			};
			_db.Tokens.Add(token);

			_db.SaveChanges();

			UserProfile profile = new UserProfile()
			{
				DisplayName = user.DisplayName,
				Token = token.Id
			};

			return profile;
		}

		public UserProfile Authenticate(AuthenticateRequest request)
		{
			string _hash = HashPassword(request.Password);
			//TODO: Check username and password
			var user = (from u in _db.Users
						 where u.Email == request.Email
								 && u.PasswordHash == _hash
						 select u).FirstOrDefault();

			if (user == null)
				throw new UnauthorizedAccessException();

			user.LastLogin = DateTime.UtcNow;

			//See if device exists
			Device device = _db.Devices.Where(d => d.Id == request.DeviceId).FirstOrDefault();
			if (device == null)
			{
				device = new Device
				{
					Id = request.DeviceId,
					AppVersion = request.AppVersion,
					CreatedOn = DateTime.UtcNow,
					LastUpdated = DateTime.UtcNow,
					Manufacturer = request.DeviceManufacturer,
					Model = request.DeviceModel,
					Os = request.DeviceOs,
					OsVersion = request.OsVersion
				};

				_db.Devices.Add(device);
				_db.SaveChanges();
			}

			//Create token
			Token token = new Token
			{
				Id = Guid.NewGuid(),
				CreatedOn = DateTime.UtcNow,
				DeviceId = device.Id,
				UserId = user.Id
			};
			_db.Tokens.Add(token);
			_db.SaveChanges();

			UserProfile profile = new UserProfile()
			{
				DisplayName = user.DisplayName,
				Token = token.Id
			};

			return profile;
		}

		public bool Validate(Guid token)
		{
			var t = _db.Tokens.FirstOrDefault(tk => tk.Id == token);

			if (t != null)
				return true;
			else
				return false;
		}

		private string HashPassword(string Password)
		{
			string _hash;
			byte[] _bytes = Encoding.Unicode.GetBytes(Password);
			SHA512 _shaM = new SHA512Managed();
			byte[] _inArray = _shaM.ComputeHash(_bytes);
			_hash = Convert.ToBase64String(_inArray);
			return _hash;
		}
		#endregion

	}
}
