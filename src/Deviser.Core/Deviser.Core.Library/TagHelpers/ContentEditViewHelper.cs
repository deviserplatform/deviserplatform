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

        private readonly INavigation _navigation;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IScopeService _scopeService;

        private readonly IViewCompilerProvider _viewCompilerProvider;
        private readonly ApplicationPartManager _applicationPartManager;

        [HtmlAttributeName(ContentEditsAttribute)]
        public string ContentEditViews { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ContentEditViewHelper(IHttpContextAccessor httpContextAccessor,
            INavigation navigation,
            IHtmlHelper htmlHelper,
            IScopeService scopeService,
            IWebHostEnvironment hostEnvironment,
            IViewCompilerProvider viewCompilerProvider,
            ApplicationPartManager applicationPartManager)
             : base(httpContextAccessor)
        {
            _htmlHelper = htmlHelper;
            _navigation = navigation;
            _scopeService = scopeService;
            _hostingEnvironment = hostEnvironment;
            _applicationPartManager = applicationPartManager;
            _viewCompilerProvider = viewCompilerProvider;
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
            var feature = new ViewsFeature();
            _applicationPartManager.PopulateFeature(feature);
            var views = feature.ViewDescriptors;
            var fp = _hostingEnvironment.ContentRootFileProvider;
            var sb = new StringBuilder();
            var editPath = string.Format(Globals.ContentTypesEditPath, _scopeService.PageContext.SelectedTheme).Replace("~","");
            var editViews = views.Where(v => v.RelativePath.Contains(editPath)).ToList();
            foreach (var contentResult in editViews.Select(editView => _htmlHelper.Partial(editView.RelativePath)).Select(GetString))
            {
                sb.Append(contentResult);
            }
            return  sb.ToString();
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }


}
