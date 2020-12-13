using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Board;
using DataModels.Models.Board.Dtos;
using DataModels.Models.WorkItems.UserStory.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
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

        public BoardsService(IRepository<KanbanBoardColumnOption> columnOptionsRepo,
            IMapper mapper,
            IRepository<KanbanBoardColumn> columnRepo)
        {
            this.columnOptionsRepo = columnOptionsRepo;
            this.mapper = mapper;
            this.columnRepo = columnRepo;
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
            var columns = await this.GetAllColumnsAsync(projectId, sprintId);

            var sprintDays = await this.columnRepo.AllAsNoTracking()
                .Where(x => x.SprintId == sprintId)
                .Include(x => x.Sprint)
                .Select(x => (x.Sprint.DueDate - x.Sprint.StartDate).TotalDays)
                .FirstOrDefaultAsync();

            var totalTasks = columns.Select(x => x.Tasks.Count);
            var totalUserStories = columns.Select(x => x.UserStories.Count);
            var totalTests = columns.Select(x => x.Tests.Count);
            var totalBugs = columns.Select(x => x.Bugs.Count);
            var totalItemsInSprint = totalUserStories.Sum() + totalTasks.Sum() + totalTests.Sum() + totalBugs.Sum();

            var finishedTasks = columns.Reverse().Take(1).Select(x => x.Tasks.Count);
            var finishedUserStories = columns.Reverse().Take(1).Select(x => x.UserStories.Count);
            var finishedTests = columns.Reverse().Take(1).Select(x => x.Tests.Count);
            var finishedBugs = columns.Reverse().Take(1).Select(X => X.Bugs.Count);
            var finishedItemsInSprint = finishedTasks.Sum() + finishedUserStories.Sum() + finishedBugs.Sum() + finishedTests.Sum();

            var remainingItems = totalItemsInSprint - finishedItemsInSprint;

            var burndownData = new BurndownViewModel()
            {
                DaysInSprint = new List<int>() { },
                TasksRemaining = new List<int>()
                {
                    totalItemsInSprint,
                },
                ScopeChanges = new List<int>(),
            };
            burndownData.TasksRemaining.Add(remainingItems);

            for (int i = int.Parse(Math.Floor(sprintDays).ToString()); i > 0 ; i--)
            {
                burndownData.DaysInSprint.Add(i);
                burndownData.ScopeChanges.Add(0);
            };

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
