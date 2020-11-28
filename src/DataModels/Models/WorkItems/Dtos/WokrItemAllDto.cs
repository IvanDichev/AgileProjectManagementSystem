namespace DataModels.Models.WorkItems.Dtos
{
    public class WokrItemAllDto
    {
        public int Id { get; set; }

        public int? WorkItemId { get; set; }

        public string Title { get; set; }

        public string WorkItemTypeType { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public int TasksCount { get; set; }
    }
}
