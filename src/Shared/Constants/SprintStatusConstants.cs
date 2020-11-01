namespace Shared.Constants
{
    public class SprintStatusConstants
    {
            // if sprint start and end date is in future
        public const string Planning = "Planning";
            //  If current date is between sprint start and end date
        public const string Active = "Active";
            // If Product Owner accepts the sprint results or Sprint date is past.
        public const string Accepted = "Accepted";
            // Sprint End Date is in the past.
        public const string Closed = "Closed";
    }
}
