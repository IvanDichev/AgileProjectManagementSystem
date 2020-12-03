using System;

namespace DataModels.Models.Sprints.Dto
{
    public class SprintInputDto
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int StatusId { get; set; }

        public int ProjectId { get; set; }
    }
}
