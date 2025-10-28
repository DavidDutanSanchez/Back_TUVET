namespace tu_vet_back.tuvet.Dtos
{
    public class PaginationBaseDto<T>
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int total { get; set; }
        public T? totalData { get; set; }

    }

    public class PaginationDto<T> : PaginationBaseDto<T>
    {
        public IQueryable<T> data { get; set; }
    }

    public class PaginationAsList<T> : PaginationBaseDto<T>
    {
        public List<T> data { get; set; }
    }
}
