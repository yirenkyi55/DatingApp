using System;
using System.Threading.Tasks;
using DatingApp.API.Data.Abstract;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Concrete
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Set the salting algorithm
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //Calculate the hash password based on the salting hash and the user plain password
                var computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                //Check if the computed password has matches password hash in the database.
                for (int i = 0; i < computedhash.Length; i++)
                {
                    if (computedhash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<User> RegisterUser(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //Add user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //Generates a passwordSalt using hmac.key
                passwordSalt = hmac.Key;

                //Compute the password hash  
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                return true;

            return false;
        }
    }
}