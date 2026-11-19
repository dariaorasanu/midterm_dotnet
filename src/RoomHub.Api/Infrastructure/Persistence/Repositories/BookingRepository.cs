using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext dbContext) : IBookingRepository
{
    public Task AddAsync(Booking booking, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        dbContext.Bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        dbContext.Bookings
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> ListForRoomAsync(Guid roomId, CancellationToken ct = default) =>
        await dbContext.Bookings
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .ToListAsync(ct);

    public Task<int> CountActiveByBookedByAndUtcDateAsync(
        string bookedBy,
        DateOnly utcDate,
        CancellationToken ct = default)
    {
        var dayStartUtc = utcDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEndInclusiveUtc = utcDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        return dbContext.Bookings
            .AsNoTracking()
            .CountAsync(
                b => b.BookedBy == bookedBy &&
                     b.Status == BookingStatus.Active &&
                     b.StartUtc >= dayStartUtc &&
                     b.StartUtc <= dayEndInclusiveUtc,
                ct);
    }
}
