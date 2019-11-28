using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data.Abstract
{
    public interface IUserRepository : IDatingRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
    }
}