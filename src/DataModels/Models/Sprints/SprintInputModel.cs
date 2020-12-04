using DataModels.Models.SprintStatuses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Sprints
{
    public class SprintInputModel
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int StatusId { get; set; }

        [Required]
        public int ProjectId{ get; set; }

        public string StatusStatus { get; set; }

        public ICollection<SprintStatusDropDown> SprintStatusDropDown { get; set; }
    }
}
