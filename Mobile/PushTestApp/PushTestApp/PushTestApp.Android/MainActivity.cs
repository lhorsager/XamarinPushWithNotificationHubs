using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PushTestApp.PushNotifications;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace PushTestApp.Droid
{
    [Activity(Label = "PushTestApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
			string deviceId = null;
			deviceId = Preferences.Get(PushTestAppStorageKey.DEVICE_ID, null);
			if (deviceId == null)
			{
				System.Diagnostics.Debug.WriteLine($"MainActivity() - DeviceId is Null - Most Likely Initial Launch - Create a DeviceId");

				Guid deviceIdGuid = Guid.NewGuid();
				deviceId = deviceIdGuid.ToString();

				System.Diagnostics.Debug.WriteLine($"MainActivity() - New DeviceId: { deviceId }");

				//register DeviceId with Xamarin.Essentials
				Preferences.Set(PushTestAppStorageKey.DEVICE_ID, deviceId);

				Task.Run(async () =>
				{
					//register DeviceId with Akavache
					//await BlobCache.UserAccount.InsertObject<Guid>(PushTestAppStorageKey.DEVICE_ID, deviceIdGuid);
				});
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"MainApplication() - DeviceId is not Null - Not Initial Launch - Have a DeviceId");

				System.Diagnostics.Debug.WriteLine($"MainApplication() - Existing DeviceId: { deviceId }");
			}

			TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

			FreshMvvm.FreshIOC.Container.Register<IMcNotificationManager>(McNotificationManager.Instance);
			IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();
			mcNotificationManager.NotificationManager = new PushTestApp.Droid.NotificationManager();

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(mcNotificationManager));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}