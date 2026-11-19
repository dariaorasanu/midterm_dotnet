using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    BookingOperationLock operationLock)
    : ICommandHandler<CreateBookingCommand, Response>
{
    public async Task<Result<Response>> Handle(CreateBookingCommand command, CancellationToken ct)
    {
        await operationLock.WaitAsync(ct);
        try
        {
            return await unitOfWork.ExecuteInTransactionAsync(async innerCt =>
            {
                var room = await rooms.GetByIdAsync(command.RoomId, innerCt);
                if (room is null)
                    return Result<Response>.Failure(RoomErrors.NotFound);

                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
                var existingRoomBookings = await bookings.ListForRoomAsync(room.Id, innerCt);

                var normalizedStartUtc = Booking.NormalizeUtc(command.StartUtc);
                var utcDate = DateOnly.FromDateTime(normalizedStartUtc);
                var normalizedBookedBy = command.BookedBy?.Trim();

                var activeBookingsOnDay = string.IsNullOrWhiteSpace(normalizedBookedBy)
                    ? 0
                    : await bookings.CountActiveByBookedByAndUtcDateAsync(normalizedBookedBy, utcDate, innerCt);

                var bookingResult = Booking.Create(
                    room,
                    command.BookedBy,
                    command.StartUtc,
                    command.EndUtc,
                    command.Attendees,
                    nowUtc,
                    existingRoomBookings,
                    activeBookingsOnDay);

                if (bookingResult.IsFailure)
                    return Result<Response>.Failure(bookingResult.Error);

                var booking = bookingResult.Value;
                await bookings.AddAsync(booking, innerCt);
                await unitOfWork.SaveChangesAsync(innerCt);

                return Result<Response>.Success(
                    new Response(booking.Id, booking.RoomId, booking.Status.ToString()));
            }, ct);
        }
        finally
        {
            operationLock.Release();
        }
    }
}
