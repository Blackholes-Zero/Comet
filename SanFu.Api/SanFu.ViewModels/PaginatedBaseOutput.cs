using System;
using System.Collections.Generic;
using System.Text;

namespace SanFu.ViewModels
{
    public class PaginatedBaseOutput<T> : BaseOutput where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<T> List { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="list">数据列表</param>
        public PaginatedBaseOutput(int pageIndex, int pageSize, int recordCount, IEnumerable<T> list)
        {
            this.List = list ?? new List<T>();
            this.PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            this.PageSize = pageSize <= 0 ? 10 : pageSize;
            this.RecordCount = recordCount <= 0 ? 0 : recordCount;
            this.TotalPages = (int)Math.Ceiling((decimal)this.RecordCount / this.PageSize);
        }
    }
}
