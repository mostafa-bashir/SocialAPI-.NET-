using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialAPI.Repositories;
using AutoMapper;
using SocialAPI.Models.Domains;
using Social.Data;
using SocialAPI.Models.DTOs;
using Social.Repositories;

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SocialDBContext dBContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;

        public AuthController(SocialDBContext dBContext, IPasswordHasher<User> passwordHasher, ITokenRepository tokenRepository,
            IMapper mapper)
        {
            this.dBContext = dBContext;
            this.passwordHasher = passwordHasher;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {

            var user = new User
            {
                Email = registerRequestDto.Email,
                Name = registerRequestDto.Name,
            };

            var hashedPassword = passwordHasher.HashPassword(user, registerRequestDto.Password);
            
            user.Password = hashedPassword;

            await dBContext.Users.AddAsync(user);

            await dBContext.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var user = await dBContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

            if (user != null)
            {
                var isPasswordCorrect = passwordHasher.VerifyHashedPassword(user, user.Password, loginRequestDto.Password);

                if (isPasswordCorrect == PasswordVerificationResult.Success) {
                    var token = tokenRepository.CreateToken(user);

                    var loginResponseDto = mapper.Map<LoginResponseDto>(user);
                    loginResponseDto.Token = token;

                    return Ok(loginResponseDto);

                }
            }

            return BadRequest();
        }
    }
}
