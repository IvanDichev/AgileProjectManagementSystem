namespace DataModels.Models.WorkItems.Tests.Dtos
{
    public class TestInputModelDto
    {
        public int IdForProject { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
        public string AcceptanceCriteria { get; set; }

        public int UserStoryId { get; set; }

        public int? UserId { get; set; }
    }
}
