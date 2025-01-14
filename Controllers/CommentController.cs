using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Social.Data;
using Social.Repositories;
using SocialAPI.Models.Domains;
using SocialAPI.Models.DTOs;

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ITokenRepository tokenRepository;
        private readonly SocialDBContext dBContext;

        public CommentController(ITokenRepository tokenRepository, SocialDBContext dBContext)
        {
            this.tokenRepository = tokenRepository;
            this.dBContext = dBContext;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto createCommentRequestDto, [FromHeader] string authorization)
        {
            var user = tokenRepository.DecodeToken(authorization);

            var comment = new Comment
            {
                PostId = createCommentRequestDto.PostId,
                UserId = user.Id,
                Script = createCommentRequestDto.Script
            };

            await dBContext.Comments.AddAsync(comment);
            await dBContext.SaveChangesAsync();

            return( Ok(comment));
        }

        // dah hn3mlo ngeb beeh el post b el comment e; hoa get A Post
        [HttpGet]
        [Route("Get/{id}")]//id = postId
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var post = await dBContext.Posts
                .Include(p => p.Comments)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            return( Ok(post));

        }
    }
}
