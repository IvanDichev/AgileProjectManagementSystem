using System.Collections.Generic;

namespace DataModels.Models.Projects
{
    public class PaginatedProjectViewModel
    {
        public ICollection<ProjectViewModel> PaginatedProjects { get; set; }

        public int RecordsPerPage { get; set; }

        public int TotalPages { get; set; }
    }
}
