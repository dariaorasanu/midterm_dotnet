# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| _2m 8s_ | _Asked AI to reorganize the API into vertical slices._ | _The Features structure and separate endpoint files._ | _No changes needed. The original routes and response formats were preserved._ |
| _2m 30s_ | _Asked AI to add Rich Domain, Result and business validations._ | _Room.Create(), Booking.Create(), Booking.Cancel() and Result<T>._ | _Moved business rules from services into the domain._ |
