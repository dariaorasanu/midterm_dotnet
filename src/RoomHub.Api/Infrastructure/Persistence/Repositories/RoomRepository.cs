using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(RoomHubDbContext dbContext) : IRoomRepository
{
    public Task AddAsync(Room room, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        dbContext.Rooms.Add(room);
        return Task.CompletedTask;
    }

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        dbContext.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);
}
