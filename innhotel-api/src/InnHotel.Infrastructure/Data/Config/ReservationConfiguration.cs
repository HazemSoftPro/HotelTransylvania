using InnHotel.Core.ReservationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnHotel.Infrastructure.Data.Config;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> res)
    {
        // Table configuration
        res.ToTable("reservations", t =>
        {
            t.HasCheckConstraint("CK_reservations_dates", "check_out_date > check_in_date");
            t.HasCheckConstraint("CK_reservations_status", "status IN ('Pending','Confirmed','Checked In','Checked Out','Cancelled')");
            t.HasCheckConstraint("CK_reservations_totalcost", "total_cost >= 0");
        });

        // Primary key
        res.HasKey(x => x.Id).HasName("reservation_id");

        // Properties
        res.Property(x => x.GuestId)
           .HasColumnName("guest_id")
           .IsRequired();

        res.Property(x => x.CheckInDate)
           .HasColumnName("check_in_date")
           .IsRequired();

        res.Property(x => x.CheckOutDate)
           .HasColumnName("check_out_date")
           .IsRequired();

        res.Property(x => x.ReservationDate)
           .HasColumnName("reservation_date")
           .HasDefaultValueSql("NOW()");

        // Status with safe string-to-enum conversion
        res.Property(x => x.Status)
           .HasColumnName("status")
           .IsRequired()
           .HasConversion(
               v => v.ToString(), // Enum -> string for saving
               v => v switch       // String -> Enum when reading
               {
                   "Pending" => ReservationStatus.Pending,
                   "Confirmed" => ReservationStatus.Confirmed,
                   "Checked In" => ReservationStatus.CheckedIn,
                   "Checked Out" => ReservationStatus.CheckedOut,
                   "Cancelled" => ReservationStatus.Cancelled,
                   _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
               }
           )
           .HasMaxLength(20);

        res.Property(x => x.TotalCost)
           .HasColumnName("total_cost")
           .HasColumnType("numeric(10,2)")
           .IsRequired();

        // Relationships
        res.HasOne(x => x.Guest)
           .WithMany()
           .HasForeignKey(x => x.GuestId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
