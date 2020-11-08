using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class CreateProjectInputModel
    {
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
    }
}
