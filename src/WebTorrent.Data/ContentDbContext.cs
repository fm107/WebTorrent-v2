using Microsoft.EntityFrameworkCore;
using WebTorrent.Model;

namespace WebTorrent.Data
{
    public sealed class ContentDbContext : DbContext
    {
        private readonly bool _created;

        public ContentDbContext(DbContextOptions<ContentDbContext> options)
            : base(options)
        {
            if (_created) return;
            Database.Migrate();
            _created = true;
        }

        public DbSet<Content> Content { get; set; }
        public DbSet<FileSystemItem> FsItem { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Content>().HasKey(m => m.Id);
            builder.Entity<Content>().HasMany(f => f.FsItems);
            builder.Entity<Content>()
                .Property(i =>
                    i.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<FileSystemItem>().HasKey(m => m.Id);
            builder.Entity<FileSystemItem>()
                .Property(i =>
                    i.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(builder);
        }
    }
}