using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Board.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.BoardColumns
{
    public class BoardColumnsService : IBoardColumnsService
    {
        private readonly IRepository<BoardColumn> repo;
        private readonly IMapper mapper;

        public BoardColumnsService(IRepository<BoardColumn> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ICollection<BoardColumnAllDto>> GetAllColumnsAsync(int projectId)
        {
            var columns = await this.repo.AllAsNoTracking()
                .Where(x => x.ProjectId == projectId)
                .ProjectTo<BoardColumnAllDto>(mapper.ConfigurationProvider).ToListAsync();

            return columns;
        }
    }
}
