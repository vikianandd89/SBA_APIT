using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Database;
using TaskManager.API.Models;

namespace TaskManager.API.Business
{
    public class UserManager : IUserManager
    {
        private readonly IUserManagerRepository userManagerRepository;
        private readonly ILogger<UserManager> logger;

        public UserManager(IUserManagerRepository userManagerRepository,
            ILogger<UserManager> logger)
        {
            this.logger = logger;
            this.userManagerRepository = userManagerRepository;
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await userManagerRepository.InsertAsync(user);
        }

        public Task DeleteUserAsync(int id)
        {
            return userManagerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await userManagerRepository.GetAllAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await userManagerRepository.GetAsync(id);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            await this.userManagerRepository.UpdateAsync(id, user);
        }
    }
}
