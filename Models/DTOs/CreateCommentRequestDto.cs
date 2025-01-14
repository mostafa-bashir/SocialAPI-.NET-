namespace SocialAPI.Models.DTOs
{
    public class CreateCommentRequestDto
    {
        public string Script { get; set; }
        public int PostId { get; set; }
    }
}
