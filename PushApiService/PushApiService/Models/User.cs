using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PushApiService.Models
{
	[Table("AppUser")]
	public class User
	{
		#region Properties
		public Guid Id { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? LastLogin { get; set; }
		public virtual ICollection<Token> Tokens { get; set; }
		#endregion

		#region Constructor
		public User()
		{
			Tokens = new HashSet<Token>();
		}
		#endregion
	}
}
