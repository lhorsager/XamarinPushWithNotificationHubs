using System;
namespace PushTestApp.PushNotifications
{
    public class McNotificationManager : IMcNotificationManager
    {
        private static IMcNotificationManager _mcNotificationManager;
        public static IMcNotificationManager Instance
        {
            get
            {
                if(_mcNotificationManager == null)
                {
                    _mcNotificationManager = new McNotificationManager();
                }

                return _mcNotificationManager;
            }
        }

        private INotificationManager _notificationManager;
        public INotificationManager NotificationManager
        {
            get
            {
                return _notificationManager;
            }
            set
            {
                _notificationManager = value;
            }
        }

    }
}
