namespace DataModels.Models.UserStories.Dtos
{
    public class UserStoryAllDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public int TasksCount { get; set; }
    }
}
