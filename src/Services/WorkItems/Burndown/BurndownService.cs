using Data.Models;
using Repo;

namespace Services.WorkItems.Burndown
{
    public class BurndownService : IBurndownService
    {
        private readonly IRepository<BurndownData> burndownRepo;
        private readonly IRepository<KanbanBoardColumn> boardRepo;

        public BurndownService(IRepository<BurndownData> burndownRepo,
            IRepository<KanbanBoardColumn> boardRepo)
        {
            this.burndownRepo = burndownRepo;
            this.boardRepo = boardRepo;
        }


    }
}