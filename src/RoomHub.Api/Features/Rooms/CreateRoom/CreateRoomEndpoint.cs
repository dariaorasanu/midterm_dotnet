using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", Handle);
    }

    private static async Task<IResult> Handle(
        Request request,
        ICommandHandler<CreateRoomCommand, Response> handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new CreateRoomCommand(request.Name, request.Capacity), ct);

        return result.Match(
            response => Results.Created($"/api/rooms/{response.Id}", response),
            error => error.ToProblem());
    }
}
