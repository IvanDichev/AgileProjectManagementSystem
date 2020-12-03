using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Sprints.Dto;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Sprints
{
    public class SprintsService : ISprintsService
    {
        private readonly IRepository<Sprint> repo;
        private readonly IMapper mapper;

        public SprintsService(IRepository<Sprint> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task CreateSprintAsync(SprintInputDto inputDto)
        {
            var sprint = new Sprint()
            {
                AddedOn = DateTime.UtcNow,
                Name = inputDto.Name,
                ProjectId = inputDto.ProjectId,
                StartDate = inputDto.StartDate,
                DueDate = inputDto.DueDate,
                //StatusId = inputDto.StatusId,
                StatusId = inputDto.StatusId == 0 ? 1 : inputDto.StatusId
            };

            await this.repo.AddAsync(sprint);
            await this.repo.SaveChangesAsync();
        }

        public async Task<ICollection<SprintDto>> GetAllForProjectAsync(int projectId)
        {
            var allSprints = await this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<SprintDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return allSprints;
        }

        public async Task<SprintDto> GetByIdAsync(int sprintId)
        {
            var sprint = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == sprintId)
                .ProjectTo<SprintDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return sprint;
        }
    }
}
