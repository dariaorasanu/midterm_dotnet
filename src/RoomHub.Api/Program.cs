using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Repositories;
using CreateBookingResponse = RoomHub.Api.Features.Bookings.CreateBooking.Response;
using CreateRoomResponse = RoomHub.Api.Features.Rooms.CreateRoom.Response;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<BookingOperationLock>();
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, CreateRoomResponse>, CreateRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand, Unit>, CancelBookingHandler>();

var app = builder.Build();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRoomsEndpoints();
app.MapBookingsEndpoints();

app.Run();
