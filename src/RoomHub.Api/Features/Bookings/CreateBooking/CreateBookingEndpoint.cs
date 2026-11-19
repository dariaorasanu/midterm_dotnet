using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Common.Http;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{roomId:guid}/bookings", Handle);
    }

    private static async Task<IResult> Handle(
        Guid roomId,
        Request request,
        ICommandHandler<CreateBookingCommand, Response> handler,
        CancellationToken ct)
    {
        var command = new CreateBookingCommand(
            roomId,
            request.BookedBy,
            request.StartUtc,
            request.EndUtc,
            request.Attendees);

        var result = await handler.Handle(command, ct);

        return result.Match(
            response => Results.Created($"/api/bookings/{response.Id}", response),
            error => error.ToProblem());
    }
}
