using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models.Users;
using DataModels.Models.Users.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> usersRepo;
        private readonly IMapper mapper;

        public UsersService(IRepository<User> usersRepo, IMapper mapper)
        {
            this.usersRepo = usersRepo;
            this.mapper = mapper;
        }

        public async Task<ICollection<UserDto>> GetPublicUsersAsync()
        {
            var publicUsers = await this.usersRepo.AllAsNoTracking()
                .Where(x => x.IsPublic == true)
                .ProjectTo<UserDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return publicUsers;
        }
    }
}
