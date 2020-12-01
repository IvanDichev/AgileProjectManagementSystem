namespace DataModels.Models.WorkItems.Tasks.Dtos
{
    public class TaskInputModelDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int UserStoryId { get; set; }

        public int? UserId { get; set; }
    }
}
