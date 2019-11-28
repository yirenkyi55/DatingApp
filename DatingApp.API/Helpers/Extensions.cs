using System;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            //Check if the user has not had his birthday yet
            if (dateOfBirth.AddYears(age) > DateTime.Today)
            {
                //We reduce the age by 1, since the person has not 
                age--;
            }

            return age;
        }
    }
}