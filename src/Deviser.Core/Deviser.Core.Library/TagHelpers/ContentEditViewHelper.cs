using Autofac;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
//using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ContentEditsAttribute)]
    public class ContentEditViewHelper : DeviserTagHelper
    {
        private const string ContentEditsAttribute = "sd-content-edits";

        private readonly INavigation _navigation;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IScopeService _scopeService;

        [HtmlAttributeName(ContentEditsAttribute)]
        public string ContentEditViews { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ContentEditViewHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor, IScopeService scopeService)
             : base(httpContextAccessor)
        {
            _htmlHelper = container.Resolve<IHtmlHelper>();
            _navigation = container.Resolve<INavigation>();
            _scopeService = scopeService;
            _hostingEnvironment = container.Resolve<IHostingEnvironment>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ContentEditViews))
            {
                output.Content.SetHtmlContent("Navigation style (sd-nav) cannot be null");
                return;
            }

            ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);

            var contentResult = GetAllEditViews();
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);
        }

        private string GetAllEditViews()
        {
            StringBuilder sb = new StringBuilder();
            string editPath = string.Format(Globals.ContentTypesEditPath, _scopeService.PageContext.SelectedTheme);
            string editViewDir = editViewDir = Path.Combine(_hostingEnvironment.ContentRootPath, editPath.Replace("~/", "").Replace("/", @"\"));
            DirectoryInfo dir = new DirectoryInfo(editViewDir);
            foreach (var file in dir.GetFiles())
            {
                if (file != null)
                {
                    var htmlContent = _htmlHelper.Partial(Path.Combine(editPath, file.Name));
                    var contentResult = GetString(htmlContent);
                    sb.Append(contentResult);
                }
            }
            return sb.ToString();
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }


}
