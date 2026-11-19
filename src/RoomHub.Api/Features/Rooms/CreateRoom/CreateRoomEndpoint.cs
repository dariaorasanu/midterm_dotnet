using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", Handle);
    }

    private static IResult Handle(Request request, RoomService service)
    {
        var room = service.Create(request.Name, request.Capacity);
        return Results.Created(
            $"/api/rooms/{room.Id}",
            new Response(room.Id, room.Name, room.Capacity));
    }
}
