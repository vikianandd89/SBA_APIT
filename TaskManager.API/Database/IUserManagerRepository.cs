using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Database
{
    public interface IUserManagerRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetAsync(int id);

        Task<int> InsertAsync(User entity);

        Task UpdateAsync(int id, User entity);

        Task DeleteAsync(int id);
    }
}
