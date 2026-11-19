using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.GetBookingById;

namespace RoomHub.Api.Features.Bookings;

public static class BookingsEndpoints
{
    public static void MapBookingsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bookings").WithTags("Bookings");

        GetBookingByIdEndpoint.Map(group);
        CancelBookingEndpoint.Map(group);
    }
}
