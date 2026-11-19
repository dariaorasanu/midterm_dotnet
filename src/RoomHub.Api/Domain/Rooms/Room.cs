using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    private Room()
    {
    }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Room>.Failure(RoomErrors.InvalidName);

        if (capacity <= 0)
            return Result<Room>.Failure(RoomErrors.InvalidCapacity);

        return Result<Room>.Success(new Room(Guid.NewGuid(), name.Trim(), capacity));
    }
}
