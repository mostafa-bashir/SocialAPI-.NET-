using Microsoft.AspNetCore.Identity;

namespace SocialAPI.Models.Domains
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
