namespace TekhneCafe.Core.Filters
{
    public class Pagination
    {
        int _page = 0;
        int _size = 10;

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value < 0 ? 0 : value;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value <= 0 ? 10 : (value > 100 ? 100 : value);
            }
        }
    }
}
