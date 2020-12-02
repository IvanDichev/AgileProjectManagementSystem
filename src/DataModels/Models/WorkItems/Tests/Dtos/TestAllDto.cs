namespace DataModels.Models.WorkItems.Tests.Dtos
{
    public class TestAllDto
    {
        public int Id { get; set; }
        public int IdForProject { get; set; }

        public string Title { get; set; }

        public int UserStoryId { get; set; }

        public int? UserId { get; set; }
    }
}
