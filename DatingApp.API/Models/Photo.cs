using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        /// <summary>
        /// The Id of the photo
        /// </summary> 
        public int Id { get; set; }

        /// <summary>
        /// The Url of the photo
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The Description of the photo
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Added date of the photo
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Allows users to indicate one of their photo as the main photo
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// The User this photo belongs to.await indicates One to many relationship(neccessary for cascade delete)
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Indicates the foreign key of the user
        /// </summary>
        public int UserId { get; set; }
    }
}