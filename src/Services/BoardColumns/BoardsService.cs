using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Board;
using DataModels.Models.Board.Dtos;
using DataModels.Models.WorkItems.UserStory.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using Shared.Constants.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public class BoardsService : IBoardsService
    {
        private readonly IRepository<KanbanBoardColumnOption> columnOptionsRepo;
        private readonly IMapper mapper;
        private readonly IRepository<KanbanBoardColumn> columnRepo;
        private readonly IRepository<BurndownData> burndownRepo;
        private readonly IRepository<Sprint> sprintRepo;

        public BoardsService(IRepository<KanbanBoardColumnOption> columnOptionsRepo,
            IMapper mapper,
            IRepository<KanbanBoardColumn> columnRepo,
            IRepository<BurndownData> burndownRepo,
            IRepository<Sprint> sprintRepo)
        {
            this.columnOptionsRepo = columnOptionsRepo;
            this.mapper = mapper;
            this.columnRepo = columnRepo;
            this.burndownRepo = burndownRepo;
            this.sprintRepo = sprintRepo;
        }

        public async Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId, int sprintId)
        {
            var columns = await this.columnRepo.AllAsNoTracking()
                .Where(x => x.KanbanBoardColumnOption.ProjectId == projectId && x.SprintId == sprintId)
                .OrderBy(x => x.KanbanBoardColumnOption.PositionLTR)
                .ProjectTo<BoardColumnAllDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return columns;
        }

        public async Task<ICollection<ColumnOptionsDto>> GetColumnOptionsAsync(int projectId)
        {
            var columns = await this.columnOptionsRepo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<ColumnOptionsDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return columns;
        }

        public async Task AddcolumnToTheLeftAsync(BoardOptionsInputModel inputModel)
        {
            var boardColumnOption = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = inputModel.ColumnName,
                MaxItems = inputModel.MaxItems,
                PositionLTR = ++inputModel.ColumnOrder,
            };

            //var column = new KanbanBoardColumn()
            //{
            //    AddedOn = DateTime.UtcNow,
            //    KanbanBoardColumnOption = boardColumnOption,
            //    //ProjectId = inputModel.ProjectId,
            //};

            // Shift already existing columns to right so there are no collisions in the order.
            await ShiftColumnPosition(inputModel.ColumnOrder, inputModel.ProjectId);

            await this.columnOptionsRepo.AddAsync(boardColumnOption);
            //await this.columnRepo.AddAsync(column);

            await this.columnOptionsRepo.SaveChangesAsync();
            //await this.columnRepo.SaveChangesAsync();
        }

        public async Task<BurndownViewModel> GetBurndownData(int projectId, int sprintId)
        {
            var burndown = await this.burndownRepo.AllAsNoTracking()
                .Where(x => x.SprintId == sprintId)
                .OrderBy(x => x.DayOfSprint)
                .ToListAsync();

            var burndownData = new BurndownViewModel()
            {
                DaysInSprint = new List<string>(),
                ScopeChanges = new List<int>(),
                TasksRemaining = new List<int>(),
            };

            //  tasksRemaining = totalTasks - finishedTasks
            //  scopeChange = totaltasksFromdayOne - totalTasksNow
            for (int i = 0; i < burndown.Count; i++)
            {
                burndownData.DaysInSprint.Add(burndown[i].DayOfSprint.ToString("dd-MMM-yyyy"));
                if (burndown[i].DayOfSprint.Date <= DateTime.UtcNow.Date)
                {
                    burndownData.ScopeChanges.Add(burndown[0].TotalTasks - burndown[i].TotalTasks);
                }
                else
                {
                    burndownData.ScopeChanges.Add(0);
                }
                if (burndown[i].DayOfSprint.Date <= DateTime.UtcNow.Date)
                {
                    if (i == 0)
                    {
                        burndownData.TasksRemaining.Add(burndown[0].TotalTasks);
                    }
                    else
                    {
                        burndownData.TasksRemaining.Add(burndown[i].TotalTasks - burndown[i].FinishedTasks);
                    }
                }
            }

            return burndownData;
        }

        private async Task ShiftColumnPosition(int columnOrder, int projectId)
        {
            var alreadyColumns = await GetColumnOptionsAsync(projectId);
            var n = columnOrder;
            foreach (var column in alreadyColumns)
            {
                if (column.PositionLTR == n)
                {
                    var oldColumn = await this.columnOptionsRepo.All().Where(x => x.Id == column.Id).FirstOrDefaultAsync();
                    oldColumn.PositionLTR++;
                    this.columnOptionsRepo.Update(oldColumn);
                    n++;
                }
            }
        }
    }
}
