using System.Linq;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using PushTestApp.PushNotifications;
using PushApiService.Dto;

namespace PushTestApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            string[] keys = message.Data.Keys.ToArray();
            string[] values = message.Data.Values.ToArray();
            
            int i = 0;
            Dictionary<string, string> messageDictionary = new Dictionary<string, string>();
            foreach(string key in keys)
            {
                messageDictionary.Add(key, values[i]);

                i++;
            }

			SendNotification(messageDictionary);
        }
        
		void SendNotification(Dictionary<string, string> message)
		{
			PushMessage pushMessage = new PushMessage();
			if(message != null)
			{
				if(message.ContainsKey("messageType"))
				{
					pushMessage.MessageType = message["messageType"];
				}

				if(message.ContainsKey("objectId"))
				{
					pushMessage.ObjectId = Guid.Parse(message["objectId"]);
				}
			}

			IMcNotificationManager mcNotificationManager = FreshMvvm.FreshIOC.Container.Resolve<IMcNotificationManager>();
            mcNotificationManager.NotificationManager.SendNotificaiton(pushMessage);
        }
    }
}