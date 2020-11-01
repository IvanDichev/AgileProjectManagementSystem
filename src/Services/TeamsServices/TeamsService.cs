using Data;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.TeamsServices
{
    public class TeamsService : ITeamsService
    {
        private readonly ApplicationDbContext db;

        public TeamsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(Team team)
        {
            var newTeam = this.db.Teams.Add(team);
            await db.SaveChangesAsync();
            return newTeam.Entity.Id;
            
        }

        public ICollection<Team> GetAllAsync()
        {
            return this.db.Teams.ToList();
        }
    }
}
