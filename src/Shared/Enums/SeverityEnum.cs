namespace Shared.Enums
{
    public enum SeverityEnum
    {
        Default = 0,
        Low = 1,
            // No loss of service. The result does not prevent the operation of the software.
        Normal = 2,
            // Minor loss of service. The result is an inconvenience, which may require a temporary workaround.
        High = 3,
            // Partial loss of service with severe impact on the business and no workaround exists.
        Urgent = 4 
            // Complete loss of service or a significant feature that is completely unavailable and no workaround exists. 
            // It does not include development issues or problems in staging environments.
    }
}
