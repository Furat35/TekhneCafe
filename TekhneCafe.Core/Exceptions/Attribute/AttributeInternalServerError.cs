namespace TekhneCafe.Core.Exceptions.Attribute
{
    public class AttributeInternalServerError : InternalServerErrorException
    {
        public AttributeInternalServerError() : base("Beklenmeyen bir hata oluştu!")
        {

        }

        public AttributeInternalServerError(string message) : base(message)
        {

        }
    }
}
