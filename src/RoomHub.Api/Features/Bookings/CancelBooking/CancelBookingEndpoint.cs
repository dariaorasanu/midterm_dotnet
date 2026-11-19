using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/cancel", Handle);
    }

    private static async Task<IResult> Handle(
        Guid id,
        ICommandHandler<CancelBookingCommand, Unit> handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new CancelBookingCommand(id), ct);

        return result.Match(
            _ => Results.NoContent(),
            error => error.ToProblem());
    }
}
