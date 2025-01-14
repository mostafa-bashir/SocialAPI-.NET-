namespace SocialAPI.Models.DTOs
{
    public class UpdatePostRequestDto
    {
        public string? Script { get; set; }
        public IFormFile[]? Files { get; set; }
    }
}
