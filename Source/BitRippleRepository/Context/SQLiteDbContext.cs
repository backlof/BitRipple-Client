using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BitRippleRepository.Context
{
	public class SQLiteDbContext : BitRippleContext
	{
		private SqliteConnectionStringBuilder SqLiteConnectionString => new SqliteConnectionStringBuilder { DataSource = Location };
		private SqliteConnection SqLiteConnection => new SqliteConnection(SqLiteConnectionString.ToString());

		public SQLiteDbContext()
		{
			Database.EnsureCreated();
			Database.Migrate();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(SqLiteConnection);
		}
	}
}