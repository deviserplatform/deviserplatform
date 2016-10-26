using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Deviser.Core.Library.Internal
{
    public class ObjectMethodExecutorCache
    {
        public ConcurrentDictionary<string, ObjectMethodExecutor> Entries { get; } =
                new ConcurrentDictionary<string, ObjectMethodExecutor>();


        public ObjectMethodExecutor GetExecutor(MethodInfo methodInfo, TypeInfo typeInfo)
        {
            ObjectMethodExecutor executor;
            var actionFullPath = typeInfo.FullName + "." + methodInfo.Name;
            if (Entries.TryGetValue(actionFullPath, out executor))
            {
                return executor;
            }

            executor = ObjectMethodExecutor.Create(methodInfo, typeInfo);
            Entries.TryAdd(actionFullPath, executor);
            return executor;
        }

    }
}
