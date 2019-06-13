using System;
namespace PushTestApp.PushNotifications
{
    public interface IMcNotificationManager
    {
        INotificationManager NotificationManager { get; set; }
    }
}
