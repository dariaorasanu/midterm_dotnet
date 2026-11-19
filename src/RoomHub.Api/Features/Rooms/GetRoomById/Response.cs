namespace RoomHub.Api.Features.Rooms.GetRoomById;

public record Response(Guid Id, string Name, int Capacity);
