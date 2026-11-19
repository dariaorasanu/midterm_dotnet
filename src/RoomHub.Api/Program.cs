using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Persistence.Repositories;
using CreateBookingResponse = RoomHub.Api.Features.Bookings.CreateBooking.Response;
using CreateRoomResponse = RoomHub.Api.Features.Rooms.CreateRoom.Response;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RoomHub")
    ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "roomhub.db")}";

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<BookingOperationLock>();

builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RoomHubDbContext>());
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, CreateRoomResponse>, CreateRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand, Unit>, CancelBookingHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (dbContext.Database.IsSqlite())
    {
        await dbContext.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");
        await dbContext.Database.ExecuteSqlRawAsync("PRAGMA busy_timeout=5000;");
    }
}

app.MapGet("/", () => "RoomHub API is running.");
app.MapRoomsEndpoints();
app.MapBookingsEndpoints();

app.Run();
