# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| _R5_ | _BookingService.cs, Create()_ | _While reading the code, I noticed there was no check for bookings starting in the past._ | _Used TimeProvider to check that the booking starts in the future._ |
| _R6_ | _BookingService.cs, Create()_ | _The maximum duration of 4 hours was not checked._ | _Added a validation that rejects bookings longer than 4 hours._ |
| _R7_ | _BookingService.cs, around line 28_ | _The code used <=, so bookings that only touched were considered overlapping._ | _Used < instead of <= so touching intervals are allowed._ |
| _R7_ | _BookingService.cs, lines 26-30_ | _AI helped me notice that cancelled bookings were also included in the overlap check._ | _Only checked active bookings for overlaps._ |
| _R8_ | _BookingService.cs, Cancel()_ | _AI pointed out that cancelling an already cancelled booking did not return an error._ | _Checked the booking status before cancelling and return a Conflict error if it is already cancelled._ |

