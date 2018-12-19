using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.ViewModels
{
    public class PaginatedBaseInput : BaseInput
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
