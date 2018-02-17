using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Common.Extensions
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
