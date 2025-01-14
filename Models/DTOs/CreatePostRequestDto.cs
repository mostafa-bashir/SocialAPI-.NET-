namespace SocialAPI.Models.DTOs
{
    public class CreatePostRequestDto
    {
        public string Script { get; set; }
        public IFormFile[]? Files { get; set; }
    }
}
