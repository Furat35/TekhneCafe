namespace TekhneCafe.Core.Exceptions.Product
{
    public class ProductInternalServerException : InternalServerErrorException
    {
        public ProductInternalServerException() : base("Beklenmeyen bir hata oluştu")
        {

        }
        public ProductInternalServerException(string message) : base(message)
        {

        }
    }
}
