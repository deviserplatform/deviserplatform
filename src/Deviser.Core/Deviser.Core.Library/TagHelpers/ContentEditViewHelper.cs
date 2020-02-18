using Deviser.Core.Common;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
//using Microsoft.Extensions.PlatformAbstractions;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = ContentEditsAttribute)]
    public class ContentEditViewHelper : DeviserTagHelper
    {
        private const string ContentEditsAttribute = "dev-content-edits";

        private readonly IHtmlHelper _htmlHelper;
        private readonly IScopeService _scopeService;

        private readonly IViewProvider _viewProvider;

        [HtmlAttributeName(ContentEditsAttribute)]
        public string ContentEditViews { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ContentEditViewHelper(IHttpContextAccessor httpContextAccessor,
            IHtmlHelper htmlHelper,
            IScopeService scopeService,
            IViewProvider viewProvider)
             : base(httpContextAccessor)
        {
            _htmlHelper = htmlHelper;
            _scopeService = scopeService;
            _viewProvider = viewProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ContentEditViews))
            {
                output.Content.SetHtmlContent("Navigation style (dev-nav) cannot be null");
                return;
            }

            ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);

            var contentResult = GetAllEditViews();
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);
        }

        private string GetAllEditViews()
        {
            var views = _viewProvider.GetCompiledViewDescriptors();
            var sb = new StringBuilder();
            var editPath = string.Format(Globals.ContentTypesEditPath, _scopeService.PageContext.SelectedTheme).Replace("~", "");
            var editViews = views.Where(v => v.RelativePath.Contains(editPath)).ToList();
            foreach (var contentResult in editViews.Select(editView => _htmlHelper.Partial(editView.RelativePath)).Select(GetString))
            {
                sb.Append(contentResult);
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
