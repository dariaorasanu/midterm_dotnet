namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> ListForRoomAsync(Guid roomId, CancellationToken ct = default);
    Task<int> CountActiveByBookedByAndUtcDateAsync(string bookedBy, DateOnly utcDate, CancellationToken ct = default);
}
