using FreshMvvm;
using Xamarin.Forms;
using PushTestApp.PageModels;
using PushTestApp.Services;
using PushTestApp.Pages;

namespace PushTestApp.PageModels.Authentication
{
	public class LoginPageModel : FreshBasePageModel
	{
		private string _email;
		private string _password;
		private ApiInstance _api;

		public LoginPageModel(ApiInstance api)
		{
            EmailAddress = "john@test.com";
            Password = "password1";
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

						Page mainPage = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
						MainNavigationPage mainNavPage = new MainNavigationPage(mainPage, "MainPage");

						CoreMethods.SwitchOutRootNavigation("MainPage");
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
