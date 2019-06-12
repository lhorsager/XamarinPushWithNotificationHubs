using FreshMvvm;
using Xamarin.Forms;

namespace PushTestApp.PageModels
{
	public class MainPageModel : FreshBasePageModel
	{
		public MainPageModel()
		{

		}

		//public Command SignupCommand
		//{
		//	get
		//	{
		//		return new Command(async () =>
		//		{
		//			var signUpPage = FreshPageModelResolver.ResolvePageModel<SignUpPageModel>();
		//			await CoreMethods.PushPageModelWithNewNavigation<SignUpPageModel>(signUpPage, true);

		//		});
		//	}
		//}
	}
}
