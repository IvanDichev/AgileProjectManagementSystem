namespace DataModels.Pagination
{
    public class PaginationFilter
    {
        private int pageNumber;
        private int pageSize;

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 50 ? 50 : value;
        }

        public PaginationFilter()
        {
            this.pageNumber = 1;
            this.pageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
