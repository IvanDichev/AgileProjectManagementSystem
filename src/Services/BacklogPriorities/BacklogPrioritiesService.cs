using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.BacklogPriorities
{
    public class BacklogPrioritiesService : IBacklogPrioritiesService
    {
        private readonly IRepository<BacklogPriority> repo;
        private readonly IMapper mapper;

        public BacklogPrioritiesService(IRepository<BacklogPriority> repo, IMapper map)
        {
            this.repo = repo;
            this.mapper = map;
        }

        // Return all projectPriorites as dto list
        public async Task<ICollection<BacklogPrioritiesDto>> GetAllAsync()
        {
            return await this.repo.All().ProjectTo<BacklogPrioritiesDto>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
