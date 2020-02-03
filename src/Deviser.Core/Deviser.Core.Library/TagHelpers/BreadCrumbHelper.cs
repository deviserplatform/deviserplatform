using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = BcAttributeName)]
    public class BreadCrumbHelper : DeviserTagHelper
    {
        private const string BcAttributeName = "dev-breadcrumb";

        private readonly IPageRepository _pageRepository;
        private readonly INavigation _navigation;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IScopeService _scopeService;

        [HtmlAttributeName(BcAttributeName)]
        public string BreadCrumbStyle { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public BreadCrumbHelper(IHttpContextAccessor httpContextAccessor,
            IScopeService scopeService,
            IPageRepository pageRepository,
            INavigation navigation,
            IHtmlHelper htmlHelper)
             : base(httpContextAccessor)
        {
            _pageRepository = pageRepository;
            _htmlHelper = htmlHelper;
            _navigation = navigation;
            _scopeService = scopeService;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(BreadCrumbStyle))
            {
                output.Content.SetHtmlContent("BreadCrumb style (dev-breadcrumb) cannot be null");
                return;
            }

            ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);
            List<Page> pages = _navigation.GetBreadCrumbs(_scopeService.PageContext.CurrentPageId);

            var htmlContent = _htmlHelper.Partial(string.Format(Globals.BreadCrumbStylePath, BreadCrumbStyle), pages);
            var contentResult = GetString(htmlContent);
            output.Content.SetHtmlContent(contentResult);
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
