using PushApiService.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PushApiService.Models
{
	[Table("Device")]
	public class Device
	{
		public Guid Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? LastUpdated { get; set; }
		public Os Os { get; set; }
		public string OsVersion { get; set; }
		public string Manufacturer { get; set; }
		public string Model { get; set; }
		public string AppVersion { get; set; }

		public virtual ICollection<Token> Tokens { get; set; }

		#region Constructor
		public Device()
		{
			Tokens = new HashSet<Token>();
		}
		#endregion
	}
}
