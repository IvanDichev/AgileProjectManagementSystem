using Repo;
using System.Threading.Tasks;

namespace Services.BurndownDatas
{
    public class BurndownData : IBurndownData
    {
        private readonly IRepository<BurndownData> bundownRepo;

        public BurndownData(IRepository<BurndownData> bundownRepo)
        {
            this.bundownRepo = bundownRepo;
        }

        public async Task UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}
