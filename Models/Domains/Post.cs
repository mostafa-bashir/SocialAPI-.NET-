using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace SocialAPI.Models.Domains
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Script { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Image> Images { get; set; }  // Navigation property to Images
        public ICollection<Comment> Comments { get; set; }

    }
}
