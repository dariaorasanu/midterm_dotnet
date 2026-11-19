using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomHubDbContext).Assembly);
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        if (Database.CurrentTransaction is not null)
            return await operation(ct);

        var connection = Database.GetDbConnection();
        var openedHere = connection.State != ConnectionState.Open;
        if (openedHere)
            await connection.OpenAsync(ct);

        try
        {
            if (connection is SqliteConnection sqliteConnection)
            {
                await using var sqliteTransaction = sqliteConnection.BeginTransaction(deferred: false);
                await Database.UseTransactionAsync(sqliteTransaction, ct);
                try
                {
                    var result = await operation(ct);
                    await sqliteTransaction.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await sqliteTransaction.RollbackAsync(CancellationToken.None);
                    throw;
                }
                finally
                {
                    await Database.UseTransactionAsync(null, CancellationToken.None);
                }
            }

            await using var transaction = await Database.BeginTransactionAsync(ct);
            try
            {
                var result = await operation(ct);
                await transaction.CommitAsync(ct);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
        finally
        {
            if (openedHere && connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }
}
