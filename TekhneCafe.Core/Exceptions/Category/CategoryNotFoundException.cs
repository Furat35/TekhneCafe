namespace TekhneCafe.Core.Exceptions.Category
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException() : base("Kategory bulunamadı!")
        {

        }

        public CategoryNotFoundException(string message) : base(message)
        {

        }
    }
}
