namespace TekhneCafe.Core.Exceptions.Product
{
    public class ProductAlreadyExistsException : BadRequestException
    {
        public ProductAlreadyExistsException() : base("Ürün mevcutta ekli!")
        {

        }
        public ProductAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
