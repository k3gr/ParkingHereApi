using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.ApplicationUsers.Entities;
using System.Reflection.Emit;

namespace ParkingHere.Infrastructure.DAL.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnType("uniqueidentifier");
        }
    }
}
