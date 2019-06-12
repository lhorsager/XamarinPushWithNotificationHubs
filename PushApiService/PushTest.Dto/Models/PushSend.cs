using System;

namespace PushApiService.Dto
{
	public class PushSend
	{
		public Guid UserId { get; set; }
		public string MessageType { get; set; }
		public Guid ObjectId { get; set; }
	}
}
