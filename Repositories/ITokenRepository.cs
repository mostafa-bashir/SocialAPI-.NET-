
using SocialAPI.Models.Domains;

namespace Social.Repositories
{
    public interface ITokenRepository
    {
        string CreateToken(User user);
        User DecodeToken(string token);

    }
}
