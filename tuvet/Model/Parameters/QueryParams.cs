
using tu_vet_back.tuvet.Constants;

namespace tu_vet_back.tuvet.Model.Parameters
{
    public class QueryParams
    {
        public string? search { get; set; }
        public int page { get; set; } = 1;
        private int _pageSize = 20;
        public int pageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > PagingConstants.MAX_PAGE_SIZE) ? PagingConstants.MAX_PAGE_SIZE : value;
            }
        }

        public bool isOrderByDescending { get; set; } = true;
        public string? orderBy { get; set; }
        public bool totalize { get; set; } = false;
        public string? fechaInicio { get; set; }
        public string? fechaFin { get; set; }
    }
}
