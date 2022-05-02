using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class PagedParams
    {
        public int MaxPageSize { get; set; } = 50;

        public int PageNumber { get; set; } = 1;


        private int _pageSize = 10;

        public int PageSize
        { 
            get =>_pageSize = 10;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
        }
    }
}
