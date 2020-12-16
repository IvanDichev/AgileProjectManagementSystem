using DataModels.Models.Users.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Users
{
    public interface IUsersService
    {
        public Task<ICollection<UserDto>> GetPublicUsersAsync();
    }
}
