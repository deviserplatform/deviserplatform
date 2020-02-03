using Deviser.Core.Common;
using Microsoft.AspNetCore.Http;

namespace Deviser.Core.Library.Extensions
{
    public static class SessionExtension
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, SDJsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : SDJsonConvert.DeserializeObject<T>(value);
        }
    }
}
