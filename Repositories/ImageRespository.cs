using Social.Data;
using SocialAPI.Models.Domains;

namespace SocialAPI.Repositories
{
    public class ImageRespository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SocialDBContext dBContext;

        public ImageRespository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, SocialDBContext dBContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
        }
        public async Task UploadAsync(IEnumerable<IFormFile> files, int postId)
        {
            foreach (var file in files)
            {
                // Generate a unique filename using a timestamp
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = $"{timestamp}{fileExtension}";

                // Set the local file path
                var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "images", fileName);

                // Upload the file to the local path
                using (var stream = new FileStream(localFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create the URL for the uploaded file
                var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/images/{fileName}";

                // Create an Image object and save to the database
                var image = new Image
                {
                    PostId = postId, // Associate with the post
                    FilePath = urlFilePath
                };

                // Save the image record to the database
                await dBContext.Images.AddAsync(image);
            }

            // Commit all changes to the database
            await dBContext.SaveChangesAsync();
        }
    }

}

