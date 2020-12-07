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
    public class BoardsService : IBoardColumnsService
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

        public async Task<ICollection<BoardColumnAllNamePositionDto>> GetColumnsNamesPositionAsync(int projectId)
        {
            var columns = await this.columnRepo.AllAsNoTracking()
                //.Where(x => x.ProjectId == projectId)
                .ProjectTo<BoardColumnAllNamePositionDto>(this.mapper.ConfigurationProvider)
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

            var column = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = boardColumnOption,
                //ProjectId = inputModel.ProjectId,
            };

            // Shift already existing columns to the left so there are no collisions in the order.
            await ShiftColumnPositionMatchesLeft(inputModel.ColumnOrder, inputModel.ProjectId);

            await this.columnOptionsRepo.AddAsync(boardColumnOption);
            await this.columnRepo.AddAsync(column);

            await this.columnOptionsRepo.SaveChangesAsync();
            await this.columnRepo.SaveChangesAsync();
        }

        private async Task ShiftColumnPositionMatchesLeft(int columnOrder, int projectId)
        {
            var alreadyColumns = await GetColumnsNamesPositionAsync(projectId);
            var n = columnOrder;
            foreach (var column in alreadyColumns)
            {
                if (column.KanbanBoardColumnOptionPositionLTR == n)
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
