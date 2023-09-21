namespace TekhneCafe.Core.Exceptions.Attribute
{
    public class AttributeAlreadyExistsException : BadRequestException
    {
        public AttributeAlreadyExistsException() : base("Attribute mevcutta var!")
        {

        }

        public AttributeAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
