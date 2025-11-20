using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations.Billing
{
    public class UserServiceConfiguration : IEntityTypeConfiguration<UserService>
    {
        public void Configure(EntityTypeBuilder<UserService> builder)
        {
            builder.HasKey(us => us.Id);

            builder.Property(us => us.UserId)
                .IsRequired();

            builder.Property(us => us.ServiceId)
                .IsRequired();

            builder.HasOne(us => us.Service)
                .WithMany(s => s.UserServices)
                .HasForeignKey(us => us.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(us => us.RemainingSessions)
                .IsRequired();

            builder.HasIndex(us => us.UserId);
            builder.HasIndex(us => us.ServiceId);

            builder.HasIndex(us => new { us.UserId, us.ServiceId })
                   .IsUnique();
        }
    }
}
