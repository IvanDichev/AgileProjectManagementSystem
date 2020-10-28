using Data.Models;
using Repo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TestService
    {
        private IRepository<Data.Models.Task> repo;
        public TestService(IRepository<Data.Models.Task> repo)
        {
            this.repo = repo;
        }

        public async System.Threading.Tasks.Task AddAsync()
        {
            await repo.AddAsync(new Data.Models.Task { Name = "TestTask" });
        }
    }
}
