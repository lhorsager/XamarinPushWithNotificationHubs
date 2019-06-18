using FreshMvvm;
using Xamarin.Forms;
using PushTestApp.PageModels;
using PushTestApp.Services;

namespace PushTestApp.PageModels.Authentication
{
	public class LoginPageModel : FreshBasePageModel
	{
		private string _email;
		private string _password;
		private ApiInstance _api;

		public LoginPageModel(ApiInstance api)
		{
			_api = api;
		}

		public string EmailAddress
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
				RaisePropertyChanged();
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				RaisePropertyChanged();
			}
		}

		public Command LoginCommand
		{
			get
			{
				return new Command(async () =>
				{
					//TODO: Check login
					try
					{
						await _api.Authenticate(_email, _password);

						var main = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
						await CoreMethods.PushPageModelWithNewNavigation<MainPageModel>(main, true);
					}
					catch (System.Exception)
					{
						//Bad login
					}
				});
			}
		}
	}
}
