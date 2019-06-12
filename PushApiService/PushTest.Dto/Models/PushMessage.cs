using System;

namespace PushApiService.Dto
{
	public class PushMessage
	{
		public string MessageType { get; set; }
		public Guid ObjectId { get; set; }
	}
}
