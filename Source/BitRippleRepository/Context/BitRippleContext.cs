using BitRippleRepository.Table;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BitRippleRepository.Context
{
	public abstract class BitRippleContext : DbContext
	{
		public static string Location => Path.Combine(Directory.GetCurrentDirectory(), "Data", "Database.db");

		internal DbSet<Filter> Filters { get; set; }
		internal DbSet<Feed> Feeds { get; set; }
		internal DbSet<Download> Downloads { get; set; }

		public void DeleteDatabase()
		{
			if (File.Exists(Location))
			{
				File.Delete(Location);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			#region Table

			modelBuilder.Entity<Feed>()
				.ToTable("Feeds");

			modelBuilder.Entity<Filter>()
				.ToTable("Filters");

			modelBuilder.Entity<Download>()
				.ToTable("Downloads");

			#endregion

			#region Primary keys

			modelBuilder.Entity<Feed>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<Filter>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<Download>()
				.HasKey(x => x.Id);

			#endregion

			#region Generate value

			modelBuilder.Entity<Feed>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Filter>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			modelBuilder.Entity<Download>()
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			#endregion

			#region Default value

			modelBuilder.Entity<Feed>()
				.Property(x => x.Id)
				.HasDefaultValue(10000);

			modelBuilder.Entity<Filter>()
				.Property(x => x.Id)
				.HasDefaultValue(20000);

			modelBuilder.Entity<Download>()
				.Property(x => x.Id)
				.HasDefaultValue(30000);

			#endregion

			#region Foreign keys

			modelBuilder.Entity<Feed>()
				.HasMany(x => x.Filters)
				.WithOne(x => x.Feed)
				.HasForeignKey(x => x.FeedId);

			modelBuilder.Entity<Filter>()
				.HasMany(x => x.Downloads)
				.WithOne(x => x.Filter)
				.HasForeignKey(x => x.FilterId);

			modelBuilder.Entity<Feed>()
				.HasMany(x => x.Downloads)
				.WithOne(x => x.Feed)
				.HasForeignKey(x => x.FeedId);

			#endregion
		}
	}
}