using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pd311_web_api.BLL.Services
{
    public class PaginationMetadata
    {
        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PaginationMetadata(int totalItems, int pageNumber, int pageSize)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

}
