using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");

    public static readonly Error InvalidBookedBy =
        Error.Validation("InvalidBookedBy", "BookedBy is required.");

    public static readonly Error InvalidRange =
        Error.Validation("InvalidRange", "The end must be after the start.");

    public static readonly Error StartInPast =
        Error.Validation("StartInPast", "A booking cannot start in the past.");

    public static readonly Error ExceedsMaxDuration =
        Error.Validation("ExceedsMaxDuration", "The maximum duration of a booking is 4 hours.");

    public static readonly Error ExceedsLargeRoomMaxDuration =
        Error.Validation("ExceedsMaxDuration", "Rooms with a capacity greater than 10 can be booked for at most 2 hours.");

    public static readonly Error InvalidAttendees =
        Error.Validation("InvalidAttendees", "Attendees must be positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("AlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error CancellationTooLate =
        Error.Conflict("CancellationTooLate", "A booking can only be cancelled at least 60 minutes before it starts.");

    public static readonly Error DailyActiveLimitReached =
        Error.Conflict("DailyActiveLimitReached", "A participant cannot have more than 2 active bookings per UTC day.");
}
