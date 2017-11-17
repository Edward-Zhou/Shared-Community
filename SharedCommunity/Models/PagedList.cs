using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models
{
    public class PagedList<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
