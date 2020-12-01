using Shared.Constants.Seeding;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryAllViewmodel
    {
        public int Id { get; set; }

        public int IdForProject { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }
    }
}
