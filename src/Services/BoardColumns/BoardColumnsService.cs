using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Board;
using DataModels.Models.Board.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public class BoardColumnsService : IBoardColumnsService
    {
        private readonly IRepository<KanbanBoardColumnOption> repo;
        private readonly IMapper mapper;

        public BoardColumnsService(IRepository<KanbanBoardColumnOption> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId)
        {
            var columns = await this.repo.AllAsNoTracking()
                //.Where(x => x.ProjectId == projectId)
                .OrderBy(x => x.PositionLTR)
                .ProjectTo<BoardColumnAllDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return columns;
        }

        public async Task<ICollection<BoardColumnAllNamePositionDto>> GetColumnsNamesPositionAsync(int projectId)
        {
            var columns = await this.repo.AllAsNoTracking()
                //.Where(x => x.ProjectId == projectId)
                .ProjectTo<BoardColumnAllNamePositionDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return columns;
        }

        public async Task AddcolumnToTheLeftAsync(BoardOptionsInputModel inputModel)
        {
            var boardColumn = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = inputModel.ColumnName,
                MaxItems = inputModel.MaxItems,
                //ProjectId = inputModel.ProjectId,
                PositionLTR = ++inputModel.ColumnOrder,
            };

            // Shift already existing columns to the left so there are no collisions in the order.
            await ShiftColumnPositionMatchesLeft(inputModel.ColumnOrder, inputModel.ProjectId);

            await this.repo.AddAsync(boardColumn);
            await this.repo.SaveChangesAsync();
        }

        private async Task ShiftColumnPositionMatchesLeft(int columnOrder, int projectId)
        {
            var alreadyColumns = await GetColumnsNamesPositionAsync(projectId);
            var n = columnOrder;
            foreach (var column in alreadyColumns)
            {
                if (column.ColumnOrder == n)
                {
                    var oldColumn = await this.repo.All().Where(x => x.Id == column.Id).FirstOrDefaultAsync();
                    oldColumn.PositionLTR++;
                    this.repo.Update(oldColumn);
                    n++;
                }
            }
        }
    }
}
