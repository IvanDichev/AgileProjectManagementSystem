using System;
using System.Collections.Generic;

namespace DataModels.Models.Projects.Dtos
{
    public class PaginatedProjectDto
    {
        public ICollection<ProjectDto> AllProjects { get; set; }

        public int RecordsPerPage { get; set; }

        public int TotalPages { get; set; }
    }
}
