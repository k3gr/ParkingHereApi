using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Infrastructure.DAL
{

    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
        {

        }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .Property(u => u.Email)
            //    .IsRequired();

            //modelBuilder.Entity<Role>()
            //    .Property(r => r.Name)
            //    .IsRequired();

            //modelBuilder.Entity<Vehicle>()
            //    .Property(v => v.Brand)
            //    .IsRequired();

            //modelBuilder.Entity<Vehicle>()
            //    .Property(v => v.Model)
            //    .IsRequired();

            //modelBuilder.Entity<Vehicle>()
            //    .Property(v => v.RegistrationPlate)
            //    .IsRequired();

            //modelBuilder.Entity<Parking>()
            //    .Property(p => p.Name)
            //    .IsRequired()
            //    .HasMaxLength(30);

            //modelBuilder.Entity<Spot>()
            //    .Property(s => s.Type)
            //    .IsRequired();

            //modelBuilder.Entity<Address>()
            //    .Property(a => a.City)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //modelBuilder.Entity<Address>()
            //    .Property(a => a.Street)
            //    .IsRequired()
            //    .HasMaxLength(50);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
