using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.Controllers
{
    public static class ViewExtensions
    {
        public static string ExecuteResultToString(this ViewResult viewResult, ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<ViewResultExecutor>();

            var resultView = executor.FindView(context, viewResult);
            resultView.EnsureSuccessful();

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


        public static string ExcuteToStringAsync(this ViewExecutor viewExecutor,
            ActionContext actionContext,
            IView view,
            ViewDataDictionary viewData,
            ITempDataDictionary tempData,
            MediaTypeHeaderValue contentType,
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
            //if (viewData == null)
            //{
            //    var metadataProvider = services.GetRequiredService<IModelMetadataProvider>();
            //    viewData = new ViewDataDictionary(metadataProvider);
            //}

            if (tempData == null)
            {
                tempData = services.GetRequiredService<ITempDataDictionary>();
            }

            //var response = actionContext.HttpContext.Response;

            if (contentType != null && contentType.Encoding == null)
            {
                // Do not modify the user supplied content type, so copy it instead
                contentType = contentType.Copy();
                contentType.Encoding = Encoding.UTF8;
            }

            // Priority list for setting content-type:
            //      1. passed in contentType (likely set by the user on the result)
            //      2. response.ContentType (likely set by the user in controller code)
            //      3. ViewExecutor.DefaultContentType (sensible default)
            //response.ContentType = contentType?.ToString() ?? response.ContentType ?? DefaultContentType.ToString();

            //if (statusCode != null)
            //{
            //    response.StatusCode = statusCode.Value;
            //}


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

            //using (var sw = new StringWriter())
            //{
            //    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
            //                                                             viewName);
            //    var viewContext = new ViewContext(ControllerContext, viewResult.View,
            //                                 ViewData, TempData, sw);
            //    viewResult.View.Render(viewContext, sw);
            //    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
            //    return sw.GetStringBuilder().ToString();
            //}

        }
    }
}
