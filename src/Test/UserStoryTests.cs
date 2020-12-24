using Data.Models;
using Moq;
using Repo;
using Services.WorkItems.UserStories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class UserStoryTests
    {
        [Fact]
        public async Task CreateAsyncShouldCreateUserStory()
        {
            var list = new List<UserStory>();
            var mockUserStoryRepo = new Mock<IRepository<UserStory>>();

            mockUserStoryRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockUserStoryRepo.Setup(x => x.AddAsync(It.IsAny<UserStory>()))
                .Callback((UserStory userStory) => list.Add(userStory));

            //var userStoryService = new UserStoryService(mockUserStoryRepo);
        }
    }
}
