using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    public const int MaxDailyActiveBookings = 2;
    public const int LargeRoomCapacityThreshold = 10;
    public static readonly TimeSpan StandardMaxDuration = TimeSpan.FromHours(4);
    public static readonly TimeSpan LargeRoomMaxDuration = TimeSpan.FromHours(2);
    public static readonly TimeSpan MinimumCancellationNotice = TimeSpan.FromMinutes(60);

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking()
    {
    }

    private Booking(
        Guid id,
        Guid roomId,
        string bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public static DateTime NormalizeUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> existingRoomBookings,
        int activeBookingsForParticipantOnUtcDay)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Result<Booking>.Failure(BookingErrors.InvalidBookedBy);

        var requestedStartUtc = NormalizeUtc(startUtc);
        var requestedEndUtc = NormalizeUtc(endUtc);
        var currentUtc = NormalizeUtc(nowUtc);

        if (requestedEndUtc <= requestedStartUtc)
            return Result<Booking>.Failure(BookingErrors.InvalidRange);

        if (requestedStartUtc < currentUtc)
            return Result<Booking>.Failure(BookingErrors.StartInPast);

        var isLargeRoom = room.Capacity > LargeRoomCapacityThreshold;
        var maxDuration = isLargeRoom ? LargeRoomMaxDuration : StandardMaxDuration;
        if (requestedEndUtc - requestedStartUtc > maxDuration)
        {
            return Result<Booking>.Failure(
                isLargeRoom ? BookingErrors.ExceedsLargeRoomMaxDuration : BookingErrors.ExceedsMaxDuration);
        }

        if (attendees <= 0)
            return Result<Booking>.Failure(BookingErrors.InvalidAttendees);

        if (attendees > room.Capacity)
            return Result<Booking>.Failure(BookingErrors.AttendeesExceedCapacity);

        var hasOverlap = existingRoomBookings.Any(existing =>
            existing.Status == BookingStatus.Active &&
            existing.StartUtc < requestedEndUtc &&
            requestedStartUtc < existing.EndUtc);

        if (hasOverlap)
            return Result<Booking>.Failure(BookingErrors.RoomOccupied);

        if (activeBookingsForParticipantOnUtcDay >= MaxDailyActiveBookings)
            return Result<Booking>.Failure(BookingErrors.DailyActiveLimitReached);

        return Result<Booking>.Success(new Booking(
            Guid.NewGuid(),
            room.Id,
            bookedBy.Trim(),
            requestedStartUtc,
            requestedEndUtc,
            attendees));
    }

    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return Result<Unit>.Failure(BookingErrors.AlreadyCancelled);

        var currentUtc = NormalizeUtc(nowUtc);
        if (StartUtc - currentUtc < MinimumCancellationNotice)
            return Result<Unit>.Failure(BookingErrors.CancellationTooLate);

        Status = BookingStatus.Cancelled;
        return Result<Unit>.Success(Unit.Value);
    }
}
