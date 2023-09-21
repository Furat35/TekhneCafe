namespace TekhneCafe.Core.Exceptions.Product
{
    public class ProductBadRequestException : BadRequestException
    {
        public ProductBadRequestException() : base("Geçersiz ürün bilgisi")
        {

        }
        public ProductBadRequestException(string message) : base(message)
        {

        }
    }
}
