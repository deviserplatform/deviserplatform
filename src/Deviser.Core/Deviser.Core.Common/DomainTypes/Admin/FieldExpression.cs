using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class FieldExpression
    {
        public ICollection<string> Parameters { get; set; }
        public string Expression { get; set; }
    }
}
