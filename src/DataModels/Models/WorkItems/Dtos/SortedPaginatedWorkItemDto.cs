using System.Collections.Generic;

namespace DataModels.Models.WorkItems.Dtos
{
    public class SortedPaginatedWorkItemDto
    {
        public ICollection<WokrItemAllDto> WorkItemViewModel { get; set; }
    }
}
