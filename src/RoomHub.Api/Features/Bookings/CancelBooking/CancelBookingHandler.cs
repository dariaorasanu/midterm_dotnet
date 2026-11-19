using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    TimeProvider timeProvider,
    BookingOperationLock operationLock)
    : ICommandHandler<CancelBookingCommand, Unit>
{
    public async Task<Result<Unit>> Handle(CancelBookingCommand command, CancellationToken ct)
    {
        await operationLock.WaitAsync(ct);
        try
        {
            var booking = await bookings.GetByIdAsync(command.BookingId, ct);
            if (booking is null)
                return Result<Unit>.Failure(BookingErrors.NotFound);

            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
            return booking.Cancel(nowUtc);
        }
        finally
        {
            operationLock.Release();
        }
    }
}
