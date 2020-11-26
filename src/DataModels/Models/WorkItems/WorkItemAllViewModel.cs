namespace DataModels.Models.WorkItems
{
    public class WorkItemAllViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public int TasksCount { get; set; }
    }
}
