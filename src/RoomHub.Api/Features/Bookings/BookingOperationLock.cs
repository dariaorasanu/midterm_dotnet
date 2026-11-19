namespace RoomHub.Api.Features.Bookings;

public sealed class BookingOperationLock : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public Task WaitAsync(CancellationToken ct) => _semaphore.WaitAsync(ct);

    public void Release() => _semaphore.Release();

    public void Dispose() => _semaphore.Dispose();
}
