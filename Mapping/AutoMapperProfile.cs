using AutoMapper;
using SocialAPI.Models.Domains;
using SocialAPI.Models.DTOs;

namespace Social.Mapping
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, LoginResponseDto>().ReverseMap();
            CreateMap<Post, CreatePostRequestDto>().ReverseMap();
        }
    }
}
