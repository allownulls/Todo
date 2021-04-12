using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Todo.Data;



namespace Todo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        DbContextOptionsBuilder<ApplicationDbContext> o = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Todo-Sergey-Drobyshev;Trusted_Connection=True;MultipleActiveResultSets=true");

        public ApplicationDbContext():base(new DbContextOptionsBuilder().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Todo-Sergey-Drobyshev;Trusted_Connection=True;MultipleActiveResultSets=true").Options)
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoHeader> TodoHeaders { get; set; }
        public DbSet<TodoLine> TodoLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper()});
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });
        }
    }
}
