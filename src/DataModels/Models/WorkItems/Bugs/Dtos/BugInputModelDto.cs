namespace DataModels.Models.WorkItems.Bugs.Dtos
{
    public class BugInputModelDto
    {
        public int IdForProject { get; set; }
     
        public string Title { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int UserStoryId { get; set; }

        public int SeverityId { get; set; }

        public int? UserId { get; set; }
    }
}
