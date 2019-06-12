using FreshMvvm;
using Xamarin.Forms;

namespace PushTestApp.PageModels.Authentication
{
	public class SignUpPageModel : FreshBasePageModel
	{
		public SignUpPageModel()
		{

		}

		public Command LoginCommand
		{
			get
			{
				return new Command(async () =>
				{
					var loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
					await CoreMethods.PushPageModelWithNewNavigation<LoginPageModel>(loginPage, true);

				});
			}
		}
	}
}
