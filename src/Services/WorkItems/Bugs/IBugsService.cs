using DataModels.Models.Severity;
using DataModels.Models.WorkItems.Bugs.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.WorkItems.Bugs
{
    public interface IBugsService
    {
        public Task<ICollection<SeverityDropDownModel>> GetSeverityDropDown();

        public Task CreateBugAsync(int projectId, BugInputModelDto inputModel);
    }
}
