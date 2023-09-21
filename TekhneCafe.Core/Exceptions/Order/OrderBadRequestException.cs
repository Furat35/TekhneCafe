namespace TekhneCafe.Core.Exceptions.Order
{
    public class OrderBadRequestException : BadRequestException
    {
        public OrderBadRequestException() : base("Geçersiz sipariş bilgisi!")
        {

        }

        public OrderBadRequestException(string message) : base(message)
        {

        }
    }
}
