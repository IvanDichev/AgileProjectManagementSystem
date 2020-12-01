namespace DataModels.Models.WorkItems.Bugs.Dtos
{
    public class BugAllDto
    {
        public int IdForProject { get; set; }

        public string Title { get; set; }

        public int UserStoryId { get; set; }

        public int SeverityId { get; set; }

        public string SeveritySeverityName { get; set; }

        public string SeverityWeight { get; set; }

        public int? UserId { get; set; }
    }
}
