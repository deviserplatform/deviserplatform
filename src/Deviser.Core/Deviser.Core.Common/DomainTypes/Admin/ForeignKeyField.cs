using Deviser.Core.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Deviser.Core.Common.DomainTypes.Admin
{
    public class ForeignKeyField
    {
        public List<ForeignKeyProperty> Properties { get; set; }

        public ForeignKeyField()
        {
            Properties = new List<ForeignKeyProperty>();
        }
    }
}
