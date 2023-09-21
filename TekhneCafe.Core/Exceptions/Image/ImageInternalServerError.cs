namespace TekhneCafe.Core.Exceptions.Image
{
    public class ImageInternalServerError : InternalServerErrorException
    {
        public ImageInternalServerError() : base("A server side error occured with Image transaction!")
        {

        }

        public ImageInternalServerError(string message) : base(message)
        {

        }
    }
}
