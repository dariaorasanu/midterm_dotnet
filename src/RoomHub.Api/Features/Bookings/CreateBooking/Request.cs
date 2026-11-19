namespace RoomHub.Api.Features.Bookings.CreateBooking;

public record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);
