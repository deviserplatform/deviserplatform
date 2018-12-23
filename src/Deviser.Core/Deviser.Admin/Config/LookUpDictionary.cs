using Deviser.Core.Common.DomainTypes.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Deviser.Admin.Config
{
    public class LookUpDictionary
    {
        private Dictionary<string, Func<List<LookUpField>>> _lookUpFunctions;

        public Dictionary<string, List<LookUpField>> LookUpData
        {
            get
            {
                if (_lookUpFunctions.Values.Count == 0)
                    return null;

                return _lookUpFunctions.ToDictionary(dic => dic.Key, dic => dic.Value());
            }
        }

        public LookUpDictionary()
        {
            _lookUpFunctions = new Dictionary<string, Func<List<LookUpField>>>();
        }

        public void Add(string key, Func<List<LookUpField>> func)
        {
            _lookUpFunctions.Add(key, func);
        }

        public bool TryGet(string key, out Func<List<LookUpField>> func)
        {
            var result = _lookUpFunctions.TryGetValue(key, out func);
            return result;
        }
    }
}
