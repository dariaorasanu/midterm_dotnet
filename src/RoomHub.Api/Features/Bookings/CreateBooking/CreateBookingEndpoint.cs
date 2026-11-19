using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{roomId:guid}/bookings", Handle);
    }

    private static IResult Handle(Guid roomId, Request request, BookingService service)
    {
        var booking = service.Create(
            roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);

        return Results.Created(
            $"/api/bookings/{booking.Id}",
            new Response(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
