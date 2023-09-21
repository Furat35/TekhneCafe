namespace TekhneCafe.Core.Exceptions.Image
{
    public class ImageAlreadyExistsException : BadRequestException
    {
        public ImageAlreadyExistsException() : base("Image already exists!")
        {

        }

        public ImageAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
