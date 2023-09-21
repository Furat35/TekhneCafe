namespace TekhneCafe.Business.Models
{
    public class MiniIORequestModel
    {
        public MiniIORequestModel(string bucketName, string objectName, Stream stream, string contentType, int length)
        {
            BucketName = bucketName;
            ObjectName = objectName;
            Stream = stream;
            ContentType = contentType;
            Length = length;
        }

        public string BucketName { get; set; }
        public string ObjectName { get; set; }
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
        public int Length { get; set; }
    }
}
