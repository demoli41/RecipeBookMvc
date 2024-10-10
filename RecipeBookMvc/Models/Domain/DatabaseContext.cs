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


    }
}
