using DataModels.Models.SprintStatuses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Sprints
{
    public class SprintInputModel
    {
        [MaxLength(75)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int StatusId { get; set; }

        public int ProjectId{ get; set; }

        public string StatusStatus { get; set; }

        public ICollection<SprintStatusDropDown> SprintStatusDropDown { get; set; }
    }
}
