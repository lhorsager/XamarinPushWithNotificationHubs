using Microsoft.EntityFrameworkCore;
using PushApiService.Models;

namespace PushApiService.Data
{
	public class PushDataContext : DbContext
	{
		#region Constructors
		public PushDataContext(DbContextOptions<PushDataContext> options) : base(options) { }
		public PushDataContext(string connectionString) : this(new DbContextOptionsBuilder()
		.UseSqlServer(connectionString).Options)
		{ }
		private PushDataContext(DbContextOptions options) : base(options)
		{
		}
		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<Token> Tokens { get; set; }
	}
}
