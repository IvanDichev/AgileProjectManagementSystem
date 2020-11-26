using DataModels.Models.Comments;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems
{
    public class UpdateWorkItemViewModel
    {
        public WorkItemViewModel ViewModel { get; set; }

        public CommentInputModel Comment { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }

        public ICollection<WorkItemTypesDropDownModel> WorkItemTypesDropDown { get; set; }
    }
}
