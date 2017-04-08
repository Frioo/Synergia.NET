using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergia.NET
{
    public class PagesInfo
    {
        public int Limit { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }
        public int TotalPages { get; }
        public int TotalMessages { get; }

        public PagesInfo(int limit, int currentPage, int nextPage, int totalPages, int totalMessages)
        {
            this.Limit = limit;
            this.CurrentPage = currentPage;
            this.NextPage = nextPage;
            this.TotalPages = totalPages;
            this.TotalMessages = totalPages;
        }
    }
}
