using System;

namespace DataModels.Models.Sprints.Dto
{
    public class SprintDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int StatusId { get; set; }
        
        public string StatusStatus { get; set; }

        public int ProjectId { get; set; }

        public int UserStoriesCount { get; set; }

        public string ProjectName { get; set; }

        public string ProjectTeamName { get; set; }
    }
}
