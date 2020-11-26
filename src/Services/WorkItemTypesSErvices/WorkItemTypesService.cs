using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.WorkItems.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.WorkItemTypesSErvices
{
    public class WorkItemTypesService : IWorkItemTypesService
    {
        private readonly IRepository<WorkItemType> repo;
        private readonly IMapper mapper;

        public WorkItemTypesService(IRepository<WorkItemType> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ICollection<WorkItemTypesDto>> GetWorkItemTypesAsync()
        {
            return await this.repo.All()
                .ProjectTo<WorkItemTypesDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
