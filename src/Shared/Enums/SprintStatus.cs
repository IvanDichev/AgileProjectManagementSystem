namespace Shared.Enums
{
    public enum SprintStatus
    {
        Default = 0,
        Planning = 1,
            // if sprint start and end date is in future
        Active = 2,
            //  If current date is between sprint start and end date
        Accepted = 3,
            // If Product Owner accepts the sprint results or Sprint date is past.
        Closed = 4
            // Sprint End Date is in the past.
    }
}
