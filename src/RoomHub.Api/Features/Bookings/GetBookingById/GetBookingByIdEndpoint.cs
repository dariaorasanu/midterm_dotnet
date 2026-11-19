using RoomHub.Api.Common.Http;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle);
    }

    private static async Task<IResult> Handle(
        Guid id,
        IBookingRepository bookings,
        CancellationToken ct)
    {
        var booking = await bookings.GetByIdAsync(id, ct);
        if (booking is null)
            return BookingErrors.NotFound.ToProblem();

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
