using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ContentEditsAttribute)]
    public class ContentEditViewHelper : DeviserTagHelper
    {
        private const string ContentEditsAttribute = "sd-content-edits";

        private INavigation navigation;
        private IHtmlHelper htmlHelper;
        IApplicationEnvironment appEnv;

        [HtmlAttributeName(ContentEditsAttribute)]
        public string ContentEditViews { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ContentEditViewHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
            this.htmlHelper = container.Resolve<IHtmlHelper>();
            this.navigation = container.Resolve<INavigation>();
            appEnv = container.Resolve<IApplicationEnvironment>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ContentEditViews))
            {
                output.Content.SetHtmlContent("Navigation style (sd-nav) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);

            var contentResult = GetAllEditViews();
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);
        }

        private string GetAllEditViews()
        {
            StringBuilder sb = new StringBuilder();
            string editViewDir = editViewDir = Path.Combine(appEnv.ApplicationBasePath, Globals.ContentTypesEditPath.Replace("~/", "").Replace("/", @"\"));
            DirectoryInfo dir = new DirectoryInfo(editViewDir);
            foreach (var file in dir.GetFiles())
            {
                if (file != null)
                {
                    var htmlContent = htmlHelper.Partial(Path.Combine(Globals.ContentTypesEditPath, file.Name));
                    var contentResult = GetString(htmlContent);
                    sb.Append(contentResult);
                }
            }
            return sb.ToString();
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }


}
