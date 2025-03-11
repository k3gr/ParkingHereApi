using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Infrastructure.DAL.Configurations
{
    public class SpotConfiguration : IEntityTypeConfiguration<Spot>
    {
        public void Configure(EntityTypeBuilder<Spot> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType("uniqueidentifier");
            builder.Property(s => s.Price)
            .HasColumnType("decimal(18,2)");
        }
    }
}
