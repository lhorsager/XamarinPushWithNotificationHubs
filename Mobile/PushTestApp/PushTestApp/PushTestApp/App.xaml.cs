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

			var loginpage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			FreshNavigationContainer freshLoginNavigationContainer = new FreshNavigationContainer(loginpage, "Login");
			freshLoginNavigationContainer.BarBackgroundColor = Color.FromHex("#00a7f7");
			MainPage = freshLoginNavigationContainer;

            

			_mcNotificationManager.NotificationManager.OnNotification += NotificationManager_OnNotification;
			_mcNotificationManager.NotificationManager.OnRegisterDevice += NotificationManager_OnRegisterDevice;

			if(Device.RuntimePlatform == Device.iOS)
			{
				App.RegisterPushNotifications.RegisterForPush();
			}
		}

		private void NotificationManager_OnNotification(object sender, NotificationEventArgs e)
		{
			if (e.NotificationObject is PushMessage)
			{
				PushMessage message = (PushMessage)e.NotificationObject;

				Debug.WriteLine($"MessageType: { message.MessageType } ObjectId: { message.ObjectId }");

				//Now we can do something with our new message in forms
			}
		}

		private void NotificationManager_OnRegisterDevice(object sender, RegisterDeviceEventArgs e)
		{
			ApiInstance api = FreshIOC.Container.Resolve<ApiInstance>();

			Task.Run(async () =>
			{
				ServiceResponseBase registerDeviceResponse = await api.RegisterDevice();
			});
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
