# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| _2m 8s_ | _Asked AI to reorganize the API into vertical slices._ | _The Features structure and separate endpoint files._ | _No changes needed. The original routes and response formats were preserved._ |
| _2m 30s_ | _Asked AI to add Rich Domain, Result and business validations._ | _Room.Create(), Booking.Create(), Booking.Cancel() and Result<T>._ | _Moved business rules from services into the domain._ |
| _1m 28s_ | _Asked AI to implement R9, limiting a participant to 2 active bookings per UTC day across all rooms._ | _The new R9 validation and the repository support needed for it. _ | _Kept the rule in the handler/domain, not in the endpoint._ |
| _2m 2s_ | _Asked a second AI (Codex) to independently review the Rich Domain, CQRS and R9 implementation._ | _The review identified a concurrency bug in R9 and two minor issues. _ | _Used the findings to request fixes before committing._ |
| _1m 45s_ | _Asked AI to fix the code review findings and implement the new R10 and R11 requirements._ | _The concurrency fix, capacity-based booking duration and cancellation deadline. _ | _Kept the business rules in the domain and used TimeProvider for cancellation._ |



