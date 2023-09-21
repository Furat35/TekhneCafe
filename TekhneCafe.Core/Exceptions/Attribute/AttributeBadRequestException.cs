namespace TekhneCafe.Core.Exceptions.Attribute
{
    public class AttributeBadRequestException : BadRequestException
    {
        public AttributeBadRequestException() : base("Geçersiz attribute bilgisi!")
        {

        }

        public AttributeBadRequestException(string message) : base(message)
        {

        }
    }
}
