using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Sprints.Dto;
using Microsoft.EntityFrameworkCore;
using Repo;
using Shared.Constants.Seeding;
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
        private readonly IRepository<SprintStatus> sprintStatusRepo;

        public SprintsService(IRepository<Sprint> sprintRepo, 
            IMapper mapper, 
            IRepository<SprintStatus> sprintStatusRepo)
        {
            this.repo = sprintRepo;
            this.mapper = mapper;
            this.sprintStatusRepo = sprintStatusRepo;
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
                //StatusId = inputDto.StatusId == 0 ? 1 : inputDto.StatusId
                StatusId = GetSprintStatus(inputDto.StartDate, inputDto.DueDate)
            };

            await this.repo.AddAsync(sprint);
            await this.repo.SaveChangesAsync();
        }

        private int GetSprintStatus(DateTime startDate, DateTime dueDate)
        {
            var now = DateTime.Now.Date;
            var relativeToStart = DateTime.Compare(now, startDate);
            var relativeToEnd = DateTime.Compare(now, dueDate);

            return (relativeToStart, relativeToEnd) switch
            {
                    // If current date is before start date -> planning
                (-1, _) => this.sprintStatusRepo.AllAsNoTracking()
                    .Where(x => x.Status == SprintStatusConstants.Planning)
                    .FirstOrDefault().Id,
                    // Else if current date is before end date -> Active
                (_, -1) => this.sprintStatusRepo.AllAsNoTracking()
                    .Where(x => x.Status == SprintStatusConstants.Active)
                    .FirstOrDefault().Id,
                    // Its last day of sprint
                (_, 0) => this.sprintStatusRepo.AllAsNoTracking()
                .Where(x => x.Status == SprintStatusConstants.Active)
                .FirstOrDefault().Id,
                    // Else -> closed
                _ => this.sprintStatusRepo.AllAsNoTracking()
                    .Where(x => x.Status == SprintStatusConstants.Closed)
                    .FirstOrDefault().Id
            };
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

        public async Task DeleteAsync(int sprintId)
        {
            var toRemove = await this.repo.All()
                .FirstOrDefaultAsync(x => x.Id == sprintId);

            this.repo.Delete(toRemove);
            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> AreUserStoriesInSprintAsync(int sprintId)
        {
            var areUserStoriesInSprint = await this.repo.AllAsNoTracking()
                .Where(x => x.Id == sprintId)
                .AnyAsync(x => x.UserStories != null);

            return areUserStoriesInSprint;
        }

        public async Task<ICollection<SprintStatusDto>> GetSprintStatusDropDownAsync()
        {
            var sprintStatuses = await this.sprintStatusRepo.AllAsNoTracking()
                .ProjectTo<SprintStatusDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return sprintStatuses;
        }
    }
}
