using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeBookMvc.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options) : base(options)
            {

              }

        public DbSet<Category> Category { get; set; }
        public DbSet<RecipeCategory> RecipeCategory { get; set; }

        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Review> Reviews { get; set; } // Новий DbSet для відгуків

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Визначення зв'язку між Recipe та Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RecipeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
