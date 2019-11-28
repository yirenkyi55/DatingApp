using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data.Seed
{
    public class SeedData
    {
        public static void SeedUsers(DataContext context)
        {
            //Check if the users table is empty before seeding data
            if (!context.Users.Any())
            {
                // Read the data from the json file
                var usersFromJson = File.ReadAllText("Data/Seed/UserSeedData.json");

                //Convert the json data into object of users
                var users = JsonConvert.DeserializeObject<List<User>>(usersFromJson);

                if (users != null)
                {
                    foreach (var user in users)
                    {
                        //Generate passwordSalt and passwordHash for each user
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash("password", out passwordHash, out passwordSalt);

                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        user.Username = user.Username.ToLower();
                        //Add user to context
                        context.Users.Add(user);

                    }
                }
                //Save changes to database
                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //Generates a passwordSalt using hmac.key
                passwordSalt = hmac.Key;

                //Compute the password hash  
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}