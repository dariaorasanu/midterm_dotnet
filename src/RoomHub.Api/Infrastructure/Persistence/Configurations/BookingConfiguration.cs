using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => Booking.NormalizeUtc(v),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.RoomId)
            .IsRequired();

        builder.Property(b => b.BookedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.StartUtc)
            .HasConversion(utcConverter)
            .IsRequired();

        builder.Property(b => b.EndUtc)
            .HasConversion(utcConverter)
            .IsRequired();

        builder.Property(b => b.Attendees)
            .IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => new { b.RoomId, b.Status });
        builder.HasIndex(b => new { b.BookedBy, b.Status, b.StartUtc });
    }
}
