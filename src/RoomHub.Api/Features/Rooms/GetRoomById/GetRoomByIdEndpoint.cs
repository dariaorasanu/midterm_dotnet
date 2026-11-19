using RoomHub.Api.Common.Http;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle);
    }

    private static async Task<IResult> Handle(
        Guid id,
        IRoomRepository rooms,
        CancellationToken ct)
    {
        var room = await rooms.GetByIdAsync(id, ct);
        if (room is null)
            return RoomErrors.NotFound.ToProblem();

        return Results.Ok(new Response(room.Id, room.Name, room.Capacity));
    }
}
