using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PushApiService.Models
{
	[Table("Token")]
	public class Token
	{
		#region Properties
		public Guid Id { get; set; }
		public Guid DeviceId { get; set; }
		[ForeignKey("DeviceId")]
		public Device Device { get; set; }
		public Guid UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		public DateTime CreatedOn { get; set; }
		#endregion
	}
}
