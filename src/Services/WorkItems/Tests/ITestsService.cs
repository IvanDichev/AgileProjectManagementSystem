using DataModels.Models.WorkItems.Tests.Dtos;
using System.Threading.Tasks;

namespace Services.WorkItems.Tests
{
    public interface ITestsService
    {
        Task CreateAsync(int projectId, TestInputModelDto inputModel);
        Task DeleteAsync(int testId);
    }
}
