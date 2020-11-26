using DataModels.Models.WorkItems.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.WorkItemTypesSErvices
{
    public interface IWorkItemTypesService
    {
        Task<ICollection<WorkItemTypesDto>> GetWorkItemTypesAsync();
    }
}
