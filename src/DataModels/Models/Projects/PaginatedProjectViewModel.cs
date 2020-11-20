using System;
using System.Collections.Generic;

namespace DataModels.Models.Projects
{
    public class PaginatedProjectViewModel
    {
        public ICollection<ProjectViewModel> AllProjects { get; set; }

        public int RecordsPerPage { get; set; }

        public int TotalPages { get; set; }
    }
}
