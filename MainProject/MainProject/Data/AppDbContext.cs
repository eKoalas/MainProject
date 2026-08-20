using MainProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MainProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //  SEED DATA 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed kullanıcılar
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 100,
                    Username = "merkez_admin",
                    Password = "123",
                    Role = "CenterUser"  
                },
                new User
                {
                    Id = 101,
                    Username = "magaza_kadikoy",
                    Password = "123",
                    Role = "StoreUser"  
                },
                new User
                {
                    Id = 102,
                    Username = "magaza_besitas",
                    Password = "123",
                    Role = "StoreUser"

                }
            );
        }





        
    }
}