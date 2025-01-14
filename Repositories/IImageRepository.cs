
namespace SocialAPI.Repositories
{
    public interface IImageRepository
    {
        public Task UploadAsync(IEnumerable<IFormFile> files, int postId);
    }
}
