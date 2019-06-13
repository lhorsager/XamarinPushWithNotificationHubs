using FreshMvvm;
using PushTestApp.PageModels.Authentication;
using Xamarin.Forms;
using PushTestApp.PushNotifications;
using PushTestApp.Interfaces;

namespace PushTestApp
{
	public partial class App : Application
	{
		//Used for iOS only to delay the push notification request
		private static IRegisterPushNotifications _registerPushNotifications;
		public static IRegisterPushNotifications RegisterPushNotifications
		{
			get
			{
				return _registerPushNotifications;
			}
			set
			{
				_registerPushNotifications = value;
			}
		}

		private IMcNotificationManager _mcNotificationManager;

		public App(IMcNotificationManager notificationManager)
		{
			_mcNotificationManager = notificationManager;

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
