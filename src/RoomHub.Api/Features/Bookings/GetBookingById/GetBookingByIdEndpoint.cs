using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle);
    }

    private static IResult Handle(Guid id, BookingService service)
    {
        var booking = service.Get(id);
        return Results.Ok(new Response(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status.ToString()));
    }
}
