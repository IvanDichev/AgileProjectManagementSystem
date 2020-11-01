using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.TeamsServices
{
    public interface ITeamsService
    {
        ICollection<Team> GetAllAsync();
        Task<int> CreateAsync(Team team);
    }
}
