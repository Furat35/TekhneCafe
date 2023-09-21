namespace TekhneCafe.Core.Exceptions.Image
{
    public class ImageNotFoundException : NotFoundException
    {
        public ImageNotFoundException() : base("Image not found exception!")
        {

        }

        public ImageNotFoundException(string message) : base(message)
        {

        }
    }
}
