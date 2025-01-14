using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace SocialAPI.Models.Domains
{
    public class Comment
    {
        public int Id { get; set; }
        public string Script { get; set; }
        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
