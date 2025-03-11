using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.ApplicationUsers.Entities;

namespace ParkingHere.Infrastructure.DAL.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnType("uniqueidentifier");
        }
    }
}
