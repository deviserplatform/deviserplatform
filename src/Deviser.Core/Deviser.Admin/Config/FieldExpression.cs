using System.Collections.Generic;

namespace Deviser.Admin.Config
{
    public class FieldExpression
    {
        public ICollection<string> Parameters { get; set; }
        public string Expression { get; set; }
    }
}
