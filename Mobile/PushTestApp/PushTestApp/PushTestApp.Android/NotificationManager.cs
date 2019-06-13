using System;
using PushApiService.Dto;
using PushTestApp.PushNotifications;

namespace PushTestApp.Droid
{
    public class NotificationManager : INotificationManager
    {
        public event RegisterDeviceHandler OnRegisterDevice;
        public event NotificationHandler OnNotification;

        public void RegisterDevice(Guid deviceId, string pushToken)
        {
            // Make sure someone is listening to event
            if (OnRegisterDevice == null) return;

            RegisterDeviceEventArgs args = new RegisterDeviceEventArgs(deviceId, pushToken);
            OnRegisterDevice(this, args);
        }

        public void SendNotificaiton(object notificationObject)
        {
            if (OnNotification == null) return;

            NotificationEventArgs args = new NotificationEventArgs(notificationObject);
            OnNotification(this, args);
        }
    }
}
