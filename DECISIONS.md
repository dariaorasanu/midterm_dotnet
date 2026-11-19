# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?


Rule R8 was originally handled in BookingService.cs, inside the Cancel() method.

In the new structure, the rule belongs in Domain/Bookings/Booking.cs, in the Booking.Cancel() method.

The booking checks its own status before changing it to Cancelled. I moved this rule into the domain because the entity should control its own state.


## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?


An occupied room is an expected business situation, not an unexpected application error.

Using Result makes it easier to return a clear error without throwing an exception.

When an Error is returned, the handler stops the operation without saving changes. The endpoint converts the error into a ProblemDetails response with HTTP 409 Conflict.


## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?


RoomHubDbContext, RoomConfiguration, BookingConfiguration and the repository implementations use EF Core.

The domain classes and command handlers do not depend directly on EF Core. They use repository interfaces and IUnitOfWork instead.

This makes the code easier to test and separates business logic from database access. The disadvantage is that we need more interfaces and classes.


## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

CreateBookingEndpoint -> CreateBookingCommand -> CreateBookingHandler -> IRoomRepository / IBookingRepository -> RoomRepository / BookingRepository -> Booking.Create() -> IBookingRepository.Add() -> IUnitOfWork.SaveChangesAsync() -> RoomHubDbContext -> SQLite.

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

Codex identified a concurrency bug in the AI-generated implementation of R9. I asked the coding agent to fix it by synchronizing booking creation and cancellation with a shared SemaphoreSlim. 

I kept this solution because it prevents concurrent requests from bypassing the booking limits within one application instance.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?


With 30 more minutes, I would add more integration tests for booking conflicts, cancellations and database persistence. The current application uses a shared SemaphoreSlim to synchronize booking operations within one instance. 

However, this does not guarantee safety when multiple application instances access the same database. I would improve database-level concurrency handling to address this risk.