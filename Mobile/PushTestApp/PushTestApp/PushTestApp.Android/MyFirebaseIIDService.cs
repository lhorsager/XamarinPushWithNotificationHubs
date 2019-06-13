using System;
using Android.App;
using Android.Util;
using WindowsAzure.Messaging;
using Firebase.Iid;
using System.Collections.Generic;
using PushTestApp.PushNotifications;

namespace PushTestApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        NotificationHub hub;

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "FCM token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            // Register with Notification Hubs
            hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);

			string deviceId = Xamarin.Essentials.Preferences.Get(PushTestAppStorageKey.DEVICE_ID, null);
			Guid deviceGuid = new Guid(deviceId);

			var tags = new List<string>() { deviceGuid.ToString() };
            var regID = hub.Register(token, tags.ToArray()).RegistrationId;

            //Log.Debug(TAG, $"Successful registration of ID {regID}");
            System.Diagnostics.Debug.WriteLine($"Successful registration of regID: { regID }");
            System.Diagnostics.Debug.WriteLine($"Successful registration of token: { token }");

            IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();
            mcNotificationManager.NotificationManager.RegisterDevice(deviceGuid, token);
        }
    }
}
