using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace BitRippleService.Repository
{
    public class SQLiteDbContext : BitRippleContext
    {
        private SqliteConnectionStringBuilder SqLiteConnectionString => new SqliteConnectionStringBuilder { DataSource = File };
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