namespace RoomHub.Api.Features.Bookings.GetBookingById;

public record Response(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);
