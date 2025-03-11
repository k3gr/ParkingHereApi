using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Infrastructure.DAL.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnType("uniqueidentifier");
            builder.Property(e => e.CreatedById).IsRequired(false);
        }
    }
}
