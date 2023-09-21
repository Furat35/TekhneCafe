using Microsoft.AspNetCore.Http;

namespace TekhneCafe.Business.Models
{
    public class UploadImageRequest
    {
        public IFormFile Image { get; set; }
        public string BucketName { get; set; }
    }
}
