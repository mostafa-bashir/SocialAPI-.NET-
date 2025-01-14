using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialAPI.Models.Domains;

namespace Social.Data
{
    public class SocialDBContext: DbContext
    {
        public SocialDBContext(DbContextOptions<SocialDBContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                 .HasOne(c => c.User)
                 .WithMany()  // User has many Comments (if this is the case)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.NoAction);  // Prevent cascading deletes

            // If you have another foreign key to another table (e.g., Posts), consider disabling cascading there too:
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments) // A Post has many Comments
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
