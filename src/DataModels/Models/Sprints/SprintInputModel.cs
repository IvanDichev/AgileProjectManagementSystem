using DataModels.Models.SprintStatuses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DataModels.Models.Sprints
{
    public class SprintInputModel
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        public string InputStartDate { get; set; }

        public DateTime ParsedStartDate => DateTime.ParseExact(this.InputStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        [Required]
        public string InputDueDate { get; set; }

        public DateTime ParsedDueDate => DateTime.ParseExact(this.InputDueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        public int StatusId { get; set; }

        [Required]
        public int ProjectId{ get; set; }

        public string StatusStatus { get; set; }

        public ICollection<SprintStatusDropDown> SprintStatusDropDown { get; set; }
    }
}
