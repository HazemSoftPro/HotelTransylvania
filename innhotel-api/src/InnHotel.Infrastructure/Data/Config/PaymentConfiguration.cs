using InnHotel.Core.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnHotel.Infrastructure.Data.Config;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Method)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.TransactionId)
            .HasMaxLength(255);

        builder.Property(p => p.PaymentProvider)
            .HasMaxLength(100);

        builder.Property(p => p.RefundedAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(p => p.IsRefunded)
            .HasDefaultValue(false);

        builder.HasOne(p => p.Reservation)
            .WithMany()
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.ReservationId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.PaymentDate);
        builder.HasIndex(p => p.TransactionId);
    }
}