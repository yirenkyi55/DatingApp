using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data.Abstract;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Concrete
{
    public class UserRepository : DatingRepository, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.Include(user => user.Photos).ToListAsync();
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users
                        .Include(user => user.Photos)
                        .FirstOrDefaultAsync(user => user.Id == userId);
            return user;
        }

    }
}