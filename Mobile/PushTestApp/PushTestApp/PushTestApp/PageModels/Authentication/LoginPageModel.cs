using FreshMvvm;
using Xamarin.Forms;
using PushTestApp.PageModels;

namespace PushTestApp.PageModels.Authentication
{
	public class LoginPageModel : FreshBasePageModel
	{
		public LoginPageModel()
		{

		}

		public Command SignupCommand
		{
			get
			{
				return new Command(async () =>
				{
					var signUpPage = FreshPageModelResolver.ResolvePageModel<SignUpPageModel>();
					await CoreMethods.PushPageModelWithNewNavigation<SignUpPageModel>(signUpPage, true);

				});
			}
		}

		public Command LoginCommand
		{
			get
			{
				return new Command(async () =>
				{
					//TODO: Check login

					var main = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
					await CoreMethods.PushPageModelWithNewNavigation<MainPageModel>(main, true);

				});
			}
		}
	}
}
