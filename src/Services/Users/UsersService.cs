//using Data.Models.Users;
//using Repo;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Services.Users
//{
//    public class UsersService
//    {
//        private readonly IRepository<User> usersRepo;

//        public UsersService(IRepository<User> usersRepo)
//        {
//            this.usersRepo = usersRepo;
//        }

//        private async Task<ICollection<UserDto>> GetUsersInProject(int projectId)
//        {
//            var users = await this.usersRepo.AllAsNoTracking()
//                .Where(x => x.TeamsUsers.Any(x => x.Team.ProjectId == projectId))
//                .ProjectTo<UserDto>(this.mapper.ConfigurationProvider)
//                .ToListAsync();

//            return users;
//        }
//    }
//}
