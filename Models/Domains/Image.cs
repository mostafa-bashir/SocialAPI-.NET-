using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialAPI.Models.Domains
{
    public class Image
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile[] File { get; set; }
        public string FilePath { get; set; }


        public int PostId { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
    }
}
