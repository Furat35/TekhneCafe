using Microsoft.Extensions.Configuration;
using Minio;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Models;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Exceptions.Image;

namespace TekhneCafe.Business.Concrete
{
    public class ImageManager : IImageService
    {
        private readonly MinioClient _minioClient;

        public ImageManager(IConfiguration configuration)
        {
            _minioClient = new MinioClient()
                .WithEndpoint(configuration.GetValue<string>("Minio:Endpoint"))
                .WithCredentials(configuration.GetValue<string>("Minio:AccessKey"), configuration.GetValue<string>("Minio:SecretKey"))
                .Build();
        }

        public async Task RemoveImageAsync(RemoveImageRequest request)
        {
            try
            {
                var bucketName = request.BucketName;
                var objectName = request.ObjectName;
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);
                await _minioClient.RemoveObjectAsync(removeObjectArgs);
            }
            catch
            {
                throw new ImageInternalServerError();
            }
        }

        public async Task<string> UploadImageAsync(UploadImageRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                throw new BadRequestException("No file uploaded");
            if (!IsImageFile(request.Image.FileName))
                throw new BadRequestException("Invalid file format. Only image files are allowed.");
            MiniIORequestModel requestModel = new MiniIORequestModel(request.BucketName, request.Image.FileName, request.Image.OpenReadStream(), request.Image.ContentType, (int)request.Image.Length);
            var filePath = await FileUploadAsync(_minioClient, requestModel);
            return filePath;
        }

        private async Task<string> FileUploadAsync(MinioClient minio, MiniIORequestModel request)
        {
            var bucketName = request.BucketName;
            var stream = request.Stream;
            var contentType = request.ContentType;
            var length = request.Length;
            try
            {
                string randomImageName = Path.GetRandomFileName().Replace(".", "");
                var objectName = randomImageName + Path.GetExtension(request.ObjectName);
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs);
                }
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithContentType(contentType)
                    .WithObjectSize(length);
                await minio.PutObjectAsync(putObjectArgs);
                return bucketName + "/" + objectName;
            }
            catch
            {
                throw new ImageInternalServerError();
            }
        }

        private bool IsImageFile(string fileName)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(fileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}

