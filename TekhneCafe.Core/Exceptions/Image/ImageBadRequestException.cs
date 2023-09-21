namespace TekhneCafe.Core.Exceptions.Image
{
    public class ImageBadRequestException : BadRequestException
    {
        public ImageBadRequestException() : base("Image bad request!")
        {

        }

        public ImageBadRequestException(string message) : base(message)
        {

        }
    }
}
