using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Common.DomainTypes
{
    public class PlaceHolder : IDisposable
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string LayoutTemplate { get; set; }
        public int SortOrder { get; set; }
        //public Module Module { get; set; }
        public Guid LayoutTypeId { get; set; }
        public List<Property> Properties { get; set; }
        public List<PlaceHolder> PlaceHolders { get; set; }

        public void Dispose()
        {
            if (Properties != null)
            {
                Properties.GetEnumerator().Dispose();
            }

            if (PlaceHolders != null)
            {
                PlaceHolders.GetEnumerator().Dispose();
            }
        }
    }
}
