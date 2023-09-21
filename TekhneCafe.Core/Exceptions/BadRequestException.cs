namespace TekhneCafe.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = "Geçersiz istek!") : base(message)
        {

        }
    }
}
