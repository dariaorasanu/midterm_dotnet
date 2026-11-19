using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(
    IRoomRepository rooms,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Response>
{
    public async Task<Result<Response>> Handle(CreateRoomCommand command, CancellationToken ct)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (roomResult.IsFailure)
            return Result<Response>.Failure(roomResult.Error);

        var room = roomResult.Value;
        await rooms.AddAsync(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<Response>.Success(new Response(room.Id, room.Name, room.Capacity));
    }
}
