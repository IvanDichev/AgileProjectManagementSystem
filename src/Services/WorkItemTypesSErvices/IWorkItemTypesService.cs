using DataModels.Models.WorkItems.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.WorkItemTypesServices
{
    public interface IWorkItemTypesService
    {
        Task<ICollection<WorkItemTypesDto>> GetWorkItemTypesAsync();
    }
}
