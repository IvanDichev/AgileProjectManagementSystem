using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Project
{
    public class CreateProjectInputModel
    {
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(350)]
        public string Description { get; set; }
    }
}
