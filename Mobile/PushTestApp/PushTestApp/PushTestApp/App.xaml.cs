using FreshMvvm;
using PushTestApp.PageModels.Authentication;
using Xamarin.Forms;
using PushTestApp.PushNotifications;
using PushTestApp.Interfaces;
using System.Threading.Tasks;
using PushTestApp.Services;
using PushApiService.Enums;
using System.Diagnostics;
using PushApiService.Dto;
using Xamarin.Essentials;
using System;
using PushTestApp.Pages;
using PushTestApp.PageModels;

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
			VersionTracking.Track();

			FreshIOC.Container.Register<ApiInstance>(new ApiInstance());

			_mcNotificationManager = notificationManager;

			InitializeComponent();

			//Check to see if we have a user Token - if we do the user has already
			//been logged in and we show the main page otherwise show login page
			string strToken = Preferences.Get("Token", "");
			if(string.IsNullOrWhiteSpace(strToken))
			{
				//show login
				var loginpage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
				FreshNavigationContainer freshLoginNavigationContainer = new FreshNavigationContainer(loginpage);
				freshLoginNavigationContainer.BarBackgroundColor = Color.FromHex("#00a7f7");
				MainPage = freshLoginNavigationContainer;
			}
			else
			{
				//show main page
				Page mp = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
				MainNavigationPage mainNavPage = new MainNavigationPage(mp, "MainPage");
				MainPage = mainNavPage;
			}

			//subscribe for notification manager notifications
			_mcNotificationManager.NotificationManager.OnNotification += NotificationManager_OnNotification;
			_mcNotificationManager.NotificationManager.OnRegisterDevice += NotificationManager_OnRegisterDevice;

			if(Device.RuntimePlatform == Device.iOS)
			{
				//This call could be done anywhere but for now we will ask for permission on initial app startup
				App.RegisterPushNotifications.RegisterForPush();
			}
		}

		#region Handle Receiving Notifications
		private void NotificationManager_OnNotification(object sender, NotificationEventArgs e)
		{
			if (e.NotificationObject is PushMessage)
			{
				PushMessage message = (PushMessage)e.NotificationObject;

				Debug.WriteLine($"MessageType: { message.MessageType } ObjectId: { message.ObjectId }");

				//Now we can do something with our new message in forms
				if(App.Current.MainPage is MainNavigationPage)
				{
					MainNavigationPage mainNavigationPage = (MainNavigationPage)App.Current.MainPage;

					if(mainNavigationPage.CurrentPage is MainPage)
					{
						MainPage p = (MainPage)mainNavigationPage.CurrentPage;
						MainPageModel pm = (MainPageModel)p.BindingContext;
						pm.PopupMessage = message.MessageType;

						//show the message type in our popup
						pm.IsShowingPopup = true;
					}
				}
			}
		}
		#endregion Handle Receiving Notifications

		#region Handle Registration of Device for Notifications
		private void NotificationManager_OnRegisterDevice(object sender, RegisterDeviceEventArgs e)
		{
			ApiInstance api = FreshIOC.Container.Resolve<ApiInstance>();

			Task.Run(async () =>
			{
				ServiceResponseBase registerDeviceResponse = await api.RegisterDevice();
			});
		}
		#endregion Handle Registration of Device for Notifications

		#region app lifecycle methods
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
		#endregion app lifecycle methods
	}
}
