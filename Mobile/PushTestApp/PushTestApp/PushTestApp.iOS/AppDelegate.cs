using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using PushTestApp.Interfaces;
using PushTestApp.PushNotifications;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;
using Xamarin.Essentials;
using PushApiService.Dto;

namespace PushTestApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IRegisterPushNotifications
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//

		private nint _finiteTaskSessionId;
		private SBNotificationHub Hub { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			FreshMvvm.FreshIOC.Container.Register<IMcNotificationManager>(McNotificationManager.Instance);

			global::Xamarin.Forms.Forms.Init();

			IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();
			mcNotificationManager.NotificationManager = new NotificationManager();

			//Setup so Xamarin.Forms can call into this Class to register for push notifications
			App.RegisterPushNotifications = this;

			LoadApplication(new App(mcNotificationManager));

            return base.FinishedLaunching(app, options);
        }

		#region Push Notifications
		public void RegisterForPush()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
				{
					if (granted)
					{
						InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
					}
				});
			}
			else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}
		}

		public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			Hub = new SBNotificationHub("Endpoint=sb://mnempushtestns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=/leIHaN0+meAt7xHE02+J4uALq13oB1cbrDCcSfSA3k=", "mnempushtest");

            Guid deviceGuid;
            string strDeviceId = Preferences.Get("DeviceId", "");
            if (String.IsNullOrEmpty(strDeviceId))
            {
                deviceGuid = Guid.NewGuid();
                Preferences.Set("DeviceId", deviceGuid.ToString());
            }
            else
            {
                deviceGuid = Guid.Parse(strDeviceId);
            }

            Hub.UnregisterAllAsync(deviceToken, (error) => {
				if (error != null)
				{
					Debug.WriteLine("Error calling Unregister: {0}", error.ToString());
					return;
				}

				NSSet tags = new NSSet(deviceGuid.ToString());
				Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) => {
					if (errorCallback != null)
						Debug.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
				});
			});

			IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();

			string strippedDeviceToken = deviceToken.ToString();
			strippedDeviceToken = strippedDeviceToken.Replace("<", "");
			strippedDeviceToken = strippedDeviceToken.Replace(">", "");

			mcNotificationManager.NotificationManager.RegisterDevice(deviceGuid, strippedDeviceToken);
		}

		public override async void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			PushMessage pushMessage = new PushMessage();

            if (null != userInfo && userInfo.ContainsKey(new NSString("messageType")))
            {
                pushMessage.MessageType = userInfo.ObjectForKey(new NSString("messageType")).ToString();
            }

            if (null != userInfo && userInfo.ContainsKey(new NSString("objectId")))
            {
                pushMessage.ObjectId = Guid.Parse(userInfo.ObjectForKey(new NSString("objectId")).ToString());
            }

			#region Background task
			if (_finiteTaskSessionId == 0)
            {
                _finiteTaskSessionId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnFiniteTaskSessionExpiration);
            }

            await Task.Run(async () =>
            {
                IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();
                mcNotificationManager.NotificationManager.SendNotificaiton(pushMessage);

				//hack to force a delay to allow code execution to complete after sending notification to Xamarin.Forms shared code
                await Task.Delay(10000);
            });

			UIApplication.SharedApplication.EndBackgroundTask(_finiteTaskSessionId);
			_finiteTaskSessionId = 0;
			#endregion Background task
		}

		private void OnFiniteTaskSessionExpiration()
		{
			Debug.WriteLine("AppDelegate.OnFiniteTaskSessionExpiration()");
		}
		#endregion Push Notifications
	}
}
