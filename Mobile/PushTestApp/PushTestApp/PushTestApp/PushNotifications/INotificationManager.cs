using System;
using PushApiService.Dto;

namespace PushTestApp.PushNotifications
{
    public interface INotificationManager
    {
        event RegisterDeviceHandler OnRegisterDevice;
        event NotificationHandler OnNotification;

        void RegisterDevice(Guid deviceId, string pushToken);
        void SendNotificaiton(object notificationObject);

        //DatabaseService DatabaseService { get; set; }
    }
}
