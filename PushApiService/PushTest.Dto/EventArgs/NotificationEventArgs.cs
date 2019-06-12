using System;

namespace PushApiService.Dto
{
	public class NotificationEventArgs : EventArgs
	{
		public object NotificationObject { get; private set; }

		public NotificationEventArgs(object notificationObject)
		{
			NotificationObject = notificationObject;
		}
	}

	public delegate void NotificationHandler(object sender, NotificationEventArgs e);
}
