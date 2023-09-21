namespace TekhneCafe.Core.Exceptions.Attribute
{
    public class AttributeNotFoundException : NotFoundException
    {
        public AttributeNotFoundException() : base("Attribute bulunamadı!")
        {

        }

        public AttributeNotFoundException(string message) : base(message)
        {

        }
    }
}
