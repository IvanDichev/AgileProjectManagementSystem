﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Sprints;
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
        private readonly IRepository<Sprint> sprintRepo;
        private readonly IMapper mapper;
        private readonly IRepository<SprintStatus> sprintStatusRepo;
        private readonly IRepository<KanbanBoardColumnOption> boardOptionsRepo;

        public SprintsService(IRepository<Sprint> sprintRepo, 
            IMapper mapper, 
            IRepository<SprintStatus> sprintStatusRepo,
            IRepository<KanbanBoardColumnOption> boardOptionsRepo)
        {
            this.sprintRepo = sprintRepo;
            this.mapper = mapper;
            this.sprintStatusRepo = sprintStatusRepo;
            this.boardOptionsRepo = boardOptionsRepo;
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
                StatusId = GetSprintStatus(inputDto.StartDate, inputDto.DueDate)
            };

            await AddKanbanColumnsForSprint(inputDto, sprint);

            await this.sprintRepo.AddAsync(sprint);
            await this.sprintRepo.SaveChangesAsync();
            
        }

        private async Task AddKanbanColumnsForSprint(SprintInputDto inputDto, Sprint sprint)
        {
            var boardOptionIdsForProject = await this.boardOptionsRepo.AllAsNoTracking()
                            .Where(x => x.ProjectId == inputDto.ProjectId)
                            .Select(x => x.Id)
                            .ToListAsync();

            foreach (var optionsId in boardOptionIdsForProject)
            {
                sprint.KanbanBoard.Add(new KanbanBoardColumn()
                {
                    KanbanBoardColumnOptionId = optionsId,
                    AddedOn = DateTime.UtcNow,
                });
            }
        }

        public async Task<ICollection<SprintDto>> GetAllForProjectAsync(int projectId)
        {
            var allSprints = await this.sprintRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<SprintDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return allSprints;
        }

        public async Task<SprintDto> GetByIdAsync(int sprintId)
        {
            var sprint = await this.sprintRepo.AllAsNoTracking()
                .Where(x => x.Id == sprintId)
                .ProjectTo<SprintDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return sprint;
        }

        public async Task DeleteAsync(int sprintId)
        {
            var toRemove = await this.sprintRepo.All()
                .FirstOrDefaultAsync(x => x.Id == sprintId);

            this.sprintRepo.Delete(toRemove);
            await this.sprintRepo.SaveChangesAsync();
        }

        public async Task<bool> AreUserStoriesInSprintAsync(int sprintId)
        {
            var areUserStoriesInSprint = await this.sprintRepo.AllAsNoTracking()
                .Where(x => x.Id == sprintId)
                .AnyAsync(x => x.UserStories != null);

            return areUserStoriesInSprint;
        }

        public async Task<ICollection<SprintDropDownModel>> GetSprintDropDownAsync(int projectId)
        {
            var sprintDropDown = await this.sprintRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<SprintDropDownModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return sprintDropDown;
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
    }
}
