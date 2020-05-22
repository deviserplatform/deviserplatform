using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Admin.Config.Filters;
using Deviser.Core.Common.Json;
using Newtonsoft.Json.Linq;

namespace Deviser.Admin.Converters
{
    public class FilterJsonConverter : JsonCreationConverter<Filter>
    {
        protected override Filter Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["isTrue"] != null && jObject["isFalse"] != null)
            {
                return new BooleanFilter();
            }

            if (jObject["date"] != null || (jObject["fromDate"] != null && jObject["toDate"] != null))
            {
                return new DateFilter();
            }

            if (jObject["number"] != null || (jObject["fromNumber"] != null && jObject["toNumber"] != null))
            {
                return new NumberFilter();
            }

            if (jObject["text"] != null)
            {
                return new TextFilter();
            }

            if (jObject["filterKeyValues"] != null)
            {
                return new SelectFilter();
            }

            return new Filter();
        }
    }
}
