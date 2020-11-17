using DataModels.Models.BacklogPriorities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BacklogPriorities
{
    public interface IBacklogPrioritiesService
    {
        Task<ICollection<BacklogPrioritiesDto>> GetAllAsync();
    }
}
