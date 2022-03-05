using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions opt): base(opt)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser()
            {
                Id = 1,
                Email = "admin@adminmail.com",
                UserName = "admin",
                Password = "Password12*",
                City = "Samsun"
            });
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser()
            {
                Id = 2,
                Email = "customer@customermail.com",
                UserName = "customer",
                Password = "Password12*",
                City = "Konya"
            });
        }
    }
}
