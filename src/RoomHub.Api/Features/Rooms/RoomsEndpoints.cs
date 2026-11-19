using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoomById;

namespace RoomHub.Api.Features.Rooms;

public static class RoomsEndpoints
{
    public static void MapRoomsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        CreateRoomEndpoint.Map(group);
        GetRoomByIdEndpoint.Map(group);
        CreateBookingEndpoint.Map(group);
    }
}
