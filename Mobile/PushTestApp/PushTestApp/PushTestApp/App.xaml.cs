using FreshMvvm;
using PushTestApp.PageModels.Authentication;
using Xamarin.Forms;

namespace PushTestApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var loginpage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			FreshNavigationContainer freshLoginNavigationContainer = new FreshNavigationContainer(loginpage, "Login");
			freshLoginNavigationContainer.BarBackgroundColor = Color.FromHex("#00a7f7");
			MainPage = freshLoginNavigationContainer;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
