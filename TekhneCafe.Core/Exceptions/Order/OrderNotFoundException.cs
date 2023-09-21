namespace TekhneCafe.Core.Exceptions.Order
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException() : base("Sipariş bulunamadı!")
        {

        }

        public OrderNotFoundException(string message) : base(message)
        {

        }
    }
}
