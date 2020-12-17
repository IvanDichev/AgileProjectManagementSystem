using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.TeamRoles.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.TeamRoles
{
    public class TeamRolesService : ITeamRolesService
    {
        private readonly IRepository<TeamRole> teamRolesRepo;
        private readonly IMapper mapper;

        public TeamRolesService(IRepository<TeamRole> teamRolesRepo, IMapper mapper)
        {
            this.teamRolesRepo = teamRolesRepo;
            this.mapper = mapper;
        }

        public async Task<ICollection<TeamRolesDto>> GetTeamRolesAsync()
        {
            var roles = await this.teamRolesRepo.AllAsNoTracking()
                .ProjectTo<TeamRolesDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return roles;
        }
    }
}
