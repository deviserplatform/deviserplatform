using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Deviser.Core.Library.Controllers
{
    //Based on Microsoft.AspNetCore.Mvc.ViewFeatures.ViewExecutor

    public static class ViewExtensions
    {
        public static readonly string DefaultContentType = "text/html; charset=utf-8";

        public static string ExecuteResultToString(this ViewResult viewResult, ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<ViewResultExecutor>();

            var resultView = executor.FindView(context, viewResult);
            resultView.EnsureSuccessful(originalLocations: null);

            var view = resultView.View;
            try
            {
                using (view as IDisposable)
                {
                    var result = executor.ExcuteToStringAsync(context, view, viewResult.ViewData,
                    viewResult.TempData,
                    viewResult.ContentType,
                    viewResult.StatusCode);
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }
            catch
            {
                throw;
            }
            return "Module cannot be loaded";
        }

        public static IHtmlContent ExecuteResultToHTML(this ViewResult viewResult, ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<ViewResultExecutor>();

            var resultView = executor.FindView(context, viewResult);
            resultView.EnsureSuccessful(originalLocations: null);

            var view = resultView.View;
            try
            {
                using (view as IDisposable)
                {
                    var result = executor.ExecuteToHTML(context, view,
                        viewResult.Model,
                        viewResult.ViewData,
                        viewResult.TempData,
                        viewResult.ContentType,
                        viewResult.StatusCode);
                    if (result != null)
                        return result;
                }
            }
            catch
            {
                throw;
            }
            return new HtmlString("Module cannot be loaded");
        }


        public static IHtmlContent ExecuteToHTML(this ViewExecutor viewExecutor,
            ActionContext actionContext,
            IView view,
            object Model,
            ViewDataDictionary viewData,
            ITempDataDictionary tempData,
            string contentType,
            int? statusCode)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var services = actionContext.HttpContext.RequestServices;

            var viewOption = services.GetRequiredService<IOptions<MvcViewOptions>>();
            var executor = services.GetRequiredService<ViewResultExecutor>();
            var htmlHelper = services.GetRequiredService<IHtmlHelper>();

            if (viewData == null)
            {
                var metadataProvider = services.GetRequiredService<IModelMetadataProvider>();
                viewData = new ViewDataDictionary(metadataProvider, actionContext.ModelState);
            }

            if (tempData == null)
            {
                tempData = services.GetRequiredService<ITempDataDictionary>();
            }

            var response = actionContext.HttpContext.Response;

            string resolvedContentType = null;
            Encoding resolvedContentTypeEncoding = null;
            ResolveContentTypeAndEncoding(
                contentType,
                response.ContentType,
                DefaultContentType,
                out resolvedContentType,
                out resolvedContentTypeEncoding);

            response.ContentType = resolvedContentType;

            if (statusCode != null)
            {
                response.StatusCode = statusCode.Value;
            }

            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewData,
                    tempData,
                    writer,
                    viewOption.Value.HtmlHelperOptions);

                ((HtmlHelper)htmlHelper).Contextualize(viewContext);

                var result = htmlHelper.Partial(view.Path, Model, viewData);

                writer.FlushAsync();

                return result;
            }
        }


        public static string ExcuteToStringAsync(this ViewExecutor viewExecutor,
            ActionContext actionContext,
            IView view,
            ViewDataDictionary viewData,
            ITempDataDictionary tempData,
            string contentType,
            int? statusCode)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            var services = actionContext.HttpContext.RequestServices;

            var viewOption = services.GetRequiredService<IOptions<MvcViewOptions>>();

            if (viewData == null)
            {
                var metadataProvider = services.GetRequiredService<IModelMetadataProvider>();
                viewData = new ViewDataDictionary(metadataProvider, actionContext.ModelState);
            }

            if (tempData == null)
            {
                tempData = services.GetRequiredService<ITempDataDictionary>();
            }

            var response = actionContext.HttpContext.Response;

            string resolvedContentType = null;
            Encoding resolvedContentTypeEncoding = null;
            ResolveContentTypeAndEncoding(
                contentType,
                response.ContentType,
                DefaultContentType,
                out resolvedContentType,
                out resolvedContentTypeEncoding);

            response.ContentType = resolvedContentType;

            if (statusCode != null)
            {
                response.StatusCode = statusCode.Value;
            }


            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewData,
                    tempData,
                    writer,
                    viewOption.Value.HtmlHelperOptions);

                //viewExecutor.DiagnosticSource.BeforeView(view, viewContext);

                view.RenderAsync(viewContext);

                //DiagnosticSource.AfterView(view, viewContext);

                // Perf: Invoke FlushAsync to ensure any buffered content is asynchronously written to the underlying
                // response asynchronously. In the absence of this line, the buffer gets synchronously written to the
                // response as part of the Dispose which has a perf impact.
                writer.FlushAsync();

                return writer.GetStringBuilder().ToString();
            }
        }


        //Based on Microsoft.AspNetCore.Mvc.Internal.ResponseContentTypeHelper.ResolveContentTypeAndEncoding

        public static void ResolveContentTypeAndEncoding(
    string actionResultContentType,
    string httpResponseContentType,
    string defaultContentType,
    out string resolvedContentType,
    out Encoding resolvedContentTypeEncoding)
        {
            Debug.Assert(defaultContentType != null);

            var defaultContentTypeEncoding = MediaType.GetEncoding(defaultContentType);
            Debug.Assert(defaultContentTypeEncoding != null);

            // 1. User sets the ContentType property on the action result
            if (actionResultContentType != null)
            {
                resolvedContentType = actionResultContentType;
                var actionResultEncoding = MediaType.GetEncoding(actionResultContentType);
                resolvedContentTypeEncoding = actionResultEncoding ?? defaultContentTypeEncoding;
                return;
            }

            // 2. User sets the ContentType property on the http response directly
            if (!string.IsNullOrEmpty(httpResponseContentType))
            {
                var mediaTypeEncoding = MediaType.GetEncoding(httpResponseContentType);
                if (mediaTypeEncoding != null)
                {
                    resolvedContentType = httpResponseContentType;
                    resolvedContentTypeEncoding = mediaTypeEncoding;
                }
                else
                {
                    resolvedContentType = httpResponseContentType;
                    resolvedContentTypeEncoding = defaultContentTypeEncoding;
                }

                return;
            }

            // 3. Fall-back to the default content type
            resolvedContentType = defaultContentType;
            resolvedContentTypeEncoding = defaultContentTypeEncoding;
        }
    }
}
