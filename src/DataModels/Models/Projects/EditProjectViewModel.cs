using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class EditProjectViewModel
    {
        public int ProjectId { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
    }
}
