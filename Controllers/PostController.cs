using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social.Data;
using SocialAPI.Models.DTOs;
using Social.Repositories;
using SocialAPI.Models.Domains;
using SocialAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly SocialDBContext dbContext;
        private readonly IMapper mapper;
        private readonly ITokenRepository tokenRepository;
        private readonly IImageRepository imageRepository;

        public PostController(SocialDBContext dbContext, IMapper mapper, ITokenRepository tokenRepository, IImageRepository imageRepository)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.tokenRepository = tokenRepository;
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] CreatePostRequestDto createPostRequestDto, [FromHeader] string authorization)
        {
            var user = tokenRepository.DecodeToken(authorization);

            var post = mapper.Map<Post>(createPostRequestDto);
            post.UserId = user.Id;

            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();

            if (createPostRequestDto.Files != null && createPostRequestDto.Files.Any())
            {
                await imageRepository.UploadAsync(createPostRequestDto.Files, post.Id);
            }

            return Ok(new { Message = "Post created successfully", PostId = post.Id });

        }

        [HttpGet()]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var posts = await dbContext.Posts
                .Include(p => p.User)  // Use lambda expressions for type-safety
                .Include(p => p.Images)  // Ensure Images are loaded as well
                .ToListAsync();

            return Ok(posts);
        }

        [Route("Update/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] int id, 
                                                [FromForm] UpdatePostRequestDto updatePostRequestDto,
                                                [FromHeader] string authorization)
        {
            var user = tokenRepository.DecodeToken(authorization);

            var post = await dbContext.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound("Post Not Found");
            }

            if (user.Id != post!.UserId)
            {
                return Unauthorized("Cannot Update Post not owned");
            }


            if (!string.IsNullOrEmpty(updatePostRequestDto.Script))
            {
                post.Script = updatePostRequestDto.Script;
            }

            if (updatePostRequestDto.Files != null && updatePostRequestDto.Files.Any())
            {
                await imageRepository.UploadAsync(updatePostRequestDto.Files, post.Id);
            }

            return Ok(post);
        }
    }
}
