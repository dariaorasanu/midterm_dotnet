using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Repositories;

public sealed class RoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = new();
    private readonly object _lock = new();

    public Task AddAsync(Room room, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_lock)
        {
            _rooms.Add(room);
        }

        return Task.CompletedTask;
    }

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        lock (_lock)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }
    }
}
