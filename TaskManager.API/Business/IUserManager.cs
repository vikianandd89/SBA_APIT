using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Business
{
    public interface IUserManager
    {
        Task<int> AddUserAsync(Models.User user);

        Task<IEnumerable<Models.User>> GetAllUserAsync();

        Task<Models.User> GetUserAsync(int id);

        Task UpdateUserAsync(int id, Models.User user);

        Task DeleteUserAsync(int id);
    }
}
