using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class CreateProjectInputModel
    {
        [MinLength(3)]
        [StringLength(40)]
        [Required]
        public string Name { get; set; }

        [StringLength(390)]
        public string Description { get; set; }
    }
}
