using BitRippleService.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BitRippleService.Repository
{
    public abstract class BitRippleContext : DbContext
    {
        public DbSet<Filter> Filters { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Download> Downloads { get; set; }

        public string File => Path.Combine(Directory.GetCurrentDirectory(), "Data", "Data.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Table

            modelBuilder.Entity<Feed>()
                 .ToTable("Feed");

            modelBuilder.Entity<Filter>()
                 .ToTable("Filter");

            modelBuilder.Entity<Download>()
                 .ToTable("Download");

            #endregion Table

            #region Primary keys

            modelBuilder.Entity<Feed>()
                 .HasKey(x => x.Id);

            modelBuilder.Entity<Filter>()
                 .HasKey(x => x.Id);

            modelBuilder.Entity<Torrent>()
                 .HasKey(x => x.Id);

            #endregion Primary keys

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

            #endregion Generate value

            #region Default value

            modelBuilder.Entity<Feed>()
                 .Property(x => x.Id)
                 .HasDefaultValue(1);

            modelBuilder.Entity<Filter>()
                 .Property(x => x.Id)
                 .ValueGeneratedOnAdd();

            modelBuilder.Entity<Download>()
                 .Property(x => x.Id)
                 .ValueGeneratedOnAdd();

            #endregion Default value

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

            #endregion Foreign keys
        }
    }
}