using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Admin.Data
{
    public class PagedResult<T>
    {
        public class PagingInfo
        {
            public int PageNo { get; set; }

            public int PageSize { get; set; }

            public int PageCount { get; set; }

            public long TotalRecordCount { get; set; }

        }
        public List<T> Data { get; private set; }

        public PagingInfo Paging { get; private set; }

        /// <summary>
        /// Constructor expects paginated items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecordCount"></param>
        public PagedResult(IEnumerable<T> items, int pageNo, int pageSize, long totalRecordCount)
        {
            Data = new List<T>(items);
            Paging = new PagingInfo
            {
                PageNo = pageNo,
                PageSize = pageSize,
                TotalRecordCount = totalRecordCount,
                PageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                    : 0
            };
        }

        /// <summary>
        /// Pagination is created in this constructor from the items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        public PagedResult(IEnumerable<T> items, int pageNo, int pageSize)
        {
            var skip = (pageNo - 1) * pageSize;
            var totalRecordCount = items.Count();
            var paging = items.Skip(skip).Take(pageSize);
            Data = new List<T>(paging);
            Paging = new PagingInfo
            {
                PageNo = pageNo,
                PageSize = pageSize,
                TotalRecordCount = totalRecordCount,
                PageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                    : 0
            };
        }
    }
}
