using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly object _lock = new();

    public Task AddAsync(Booking booking, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_lock)
        {
            _bookings.Add(booking);
        }

        return Task.CompletedTask;
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_lock)
        {
            return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
        }
    }

    public Task<IReadOnlyList<Booking>> ListForRoomAsync(Guid roomId, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_lock)
        {
            IReadOnlyList<Booking> result = _bookings.Where(b => b.RoomId == roomId).ToList();
            return Task.FromResult(result);
        }
    }

    public Task<int> CountActiveByBookedByAndUtcDateAsync(
        string bookedBy,
        DateOnly utcDate,
        CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var dayStartUtc = utcDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEndInclusiveUtc = utcDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        lock (_lock)
        {
            var count = _bookings.Count(b =>
                b.BookedBy == bookedBy &&
                b.Status == BookingStatus.Active &&
                b.StartUtc >= dayStartUtc &&
                b.StartUtc <= dayEndInclusiveUtc);

            return Task.FromResult(count);
        }
    }
}
