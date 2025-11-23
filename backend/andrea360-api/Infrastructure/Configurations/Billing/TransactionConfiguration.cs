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
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.ServiceId)
                .IsRequired();

            builder.HasOne(t => t.Service)
                .WithMany(s => s.Transactions) 
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Amount)
                .HasPrecision(18, 2)   
                .IsRequired();

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.StripeTransactionId)
                .HasMaxLength(255)
                .IsRequired(false); 

            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.ServiceId);
            builder.HasIndex(t => t.TransactionDate);
        }
    }
}
