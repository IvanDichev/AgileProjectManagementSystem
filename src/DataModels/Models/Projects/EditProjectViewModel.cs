using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class EditProjectViewModel
    {
        public int Id { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
    }
}
