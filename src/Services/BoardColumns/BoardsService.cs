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
            //  tasksRemaining = totalTasks - finishedTasks
            //  scopeChange = totaltasksFromdayOne - totalTasksNow

            //  id  DayNo   sprintId    totalTasks  finishedTasks
            //  1   1       1           10          0
            //  2   2       1           10          1
            //  3   3       1           10          3
            //  4   4       1           11          4
            //  5   5       1           11          6
            //  6   6       1           11          9
            //  7   7       1           11          11
            //  8   1       2           7           1 sch 0 - tr 6
            //  9   2       2           8           1
            var burndownDatatable = new BurndownData();

            var columns = await this.GetAllColumnsAsync(projectId, sprintId);

            var sprintDays = await this.columnRepo.AllAsNoTracking()
                .Where(x => x.SprintId == sprintId)
                .Include(x => x.Sprint)
                .Select(x => (x.Sprint.DueDate - x.Sprint.StartDate).TotalDays)
                .FirstOrDefaultAsync();
            ;
            var sprints = await this.sprintRepo.AllAsNoTracking()
                .Where(x => x.Status.Status == SprintStatusConstants.Active ||
                            x.Status.Status == SprintStatusConstants.Planning)
                .Include(x => x.KanbanBoard)
                .ThenInclude(x => x.Tasks)
                .Include(x => x.KanbanBoard)
                .ThenInclude(x => x.UserStories)
                .Include(x => x.KanbanBoard)
                .ThenInclude(x => x.Tests)
                .Include(x => x.KanbanBoard)
                .ThenInclude(x => x.Bugs)
                .ToListAsync();
            ;
            foreach (var sprint in sprints)
            {
                var tasks = sprint.KanbanBoard.Select(x => x.Tasks.Count).Sum();
                var userStories = sprint.KanbanBoard.Select(x => x.UserStories.Count).Sum();
                var tests = sprint.KanbanBoard.Select(x => x.Tests.Count).Sum();
                var bugs = sprint.KanbanBoard.Select(x => x.Bugs.Count).Sum();
            }

            // Get total items in sprint
            var totalTasks = columns.Select(x => x.Tasks.Count);
            var totalUserStories = columns.Select(x => x.UserStories.Count);
            var totalTests = columns.Select(x => x.Tests.Count);
            var totalBugs = columns.Select(x => x.Bugs.Count);
            var totalItemsInSprint = totalUserStories.Sum() + totalTasks.Sum() + totalTests.Sum() + totalBugs.Sum();

            // Get finished items in sprint
            // TODO fix this to get finished items for everyday from burndownData table in Db
            var finishedTasks = columns.Reverse().Take(1).Select(x => x.Tasks.Count);
            var finishedUserStories = columns.Reverse().Take(1).Select(x => x.UserStories.Count);
            var finishedTests = columns.Reverse().Take(1).Select(x => x.Tests.Count);
            var finishedBugs = columns.Reverse().Take(1).Select(X => X.Bugs.Count);
            var finishedItemsInSprint = finishedTasks.Sum() + finishedUserStories.Sum() + finishedBugs.Sum() + finishedTests.Sum();

            // For all sprints which status is active or planning
            burndownDatatable.AddedOn = DateTime.UtcNow;
            burndownDatatable.TotalTasks = totalItemsInSprint;
            burndownDatatable.FinishedTasks = finishedItemsInSprint;
            burndownDatatable.SprintId = sprintId;
            //burndownDatatable.DayNo = DateTime.UtcNow.Date;

            // Remaining items
            var remainingItems = totalItemsInSprint - finishedItemsInSprint;

            // Initialize burndown data collection
            var burndownData = new BurndownViewModel()
            {
                DaysInSprint = new List<string>() { },
                TasksRemaining = new List<int>()
                {
                    totalItemsInSprint,
                },
                ScopeChanges = new List<int>(),
            };

            // Add remaining items to colleciton tracking remainings 
            // TODO fix this to get remaining tasks everyday from burndownData table in Db
            burndownData.TasksRemaining.Add(remainingItems);

            // Add days in burndown data and scope changes 
            // TODO fix this to get scope changes from burndowData table in Db
            //for (int i = int.Parse(Math.Floor(sprintDays).ToString()); i > 0 ; i--)
            //{
            //    burndownData.DaysInSprint.Add(DateTime.UtcNow.Date.AddDays(i));
            //    burndownData.ScopeChanges.Add(0);
            //};
            // if sprint is one day add the final date!!
            for (int i = 0; i < int.Parse(Math.Ceiling(sprintDays).ToString()) + 1 ; i++)
            {
                burndownData.DaysInSprint.Add(DateTime.UtcNow.Date.AddDays(i).ToString("dd/MMM/yyyy"));
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
