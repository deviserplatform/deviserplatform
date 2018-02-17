using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Common.DomainTypes
{
    public class PageLayout : IDisposable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PlaceHolder> PlaceHolders { get; set; }
        public Guid PageId { get; set; }
        public bool IsChanged { get; set; }
        public bool IsDeleted { get; set; }

        public void Dispose()
        {
            if (PlaceHolders != null)
            {
                PlaceHolders.GetEnumerator().Dispose();
            }
        }
    }
}
