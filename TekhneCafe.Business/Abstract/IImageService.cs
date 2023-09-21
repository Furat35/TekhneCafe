using TekhneCafe.Business.Models;

namespace TekhneCafe.Business.Abstract
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(UploadImageRequest request);
        Task RemoveImageAsync(RemoveImageRequest request);
    }
}
