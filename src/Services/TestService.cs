using Repo;

namespace Services
{
    public class TestService
    {
        private IRepository<Data.Models.Assignment> repo;
        public TestService(IRepository<Data.Models.Assignment> repo)
        {
            this.repo = repo;
        }

        public async System.Threading.Tasks.Task AddAsync()
        {
            await repo.AddAsync(new Data.Models.Assignment { Name = "TestTask" });
        }
    }
}
