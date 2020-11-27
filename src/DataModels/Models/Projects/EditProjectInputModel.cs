using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class EditProjectInputModel
    {
        public int ProjectId { get; set; }

        [StringLength(390)]
        public string Description { get; set; }
    }
}
