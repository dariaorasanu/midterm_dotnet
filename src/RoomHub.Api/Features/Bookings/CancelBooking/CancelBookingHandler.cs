using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    BookingOperationLock operationLock)
    : ICommandHandler<CancelBookingCommand, Unit>
{
    public async Task<Result<Unit>> Handle(CancelBookingCommand command, CancellationToken ct)
    {
        await operationLock.WaitAsync(ct);
        try
        {
            return await unitOfWork.ExecuteInTransactionAsync(async innerCt =>
            {
                var booking = await bookings.GetByIdAsync(command.BookingId, innerCt);
                if (booking is null)
                    return Result<Unit>.Failure(BookingErrors.NotFound);

                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
                var cancelResult = booking.Cancel(nowUtc);
                if (cancelResult.IsFailure)
                    return cancelResult;

                await unitOfWork.SaveChangesAsync(innerCt);
                return cancelResult;
            }, ct);
        }
        finally
        {
            operationLock.Release();
        }
    }
}
