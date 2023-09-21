namespace TekhneCafe.Core.Exceptions.Product
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException() : base("Ürün bulunamadı!")
        {

        }
        public ProductNotFoundException(string message) : base(message)
        {

        }
    }
}
