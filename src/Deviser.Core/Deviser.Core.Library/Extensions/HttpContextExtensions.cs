using System;
using Microsoft.AspNetCore.Http;

namespace Deviser.Core.Library.Extensions
{
    public static class HttpContextExtensions
    {
        public static T RegisterForDispose<T>(this T disposable, HttpContext context) where T : IDisposable
        {
            context.Response.RegisterForDispose(disposable);
            return disposable;
        }
    }
}
