using DataModels.Models.WorkItems.Bugs.Dtos;
using DataModels.Models.WorkItems.Tasks.Dtos;
using DataModels.Models.WorkItems.Tests.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryAllViewmodel
    {
        public int Id { get; set; }

        public int IdForProject { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string SprintName { get; set; }

        public string ShortSprintName => this.SprintName?.Length > 13 ? SprintName.Substring(0, 13) + "..." : SprintName;

        public ICollection<TaskAllDto> Tasks { get; set; }

        public ICollection<TestAllDto> Tests { get; set; }

        public ICollection<BugAllDto> Bugs { get; set; }
    }
}
