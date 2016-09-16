using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = BcAttributeName)]
    public class BreadCrumbHelper : DeviserTagHelper
    {
        private const string BcAttributeName = "sd-breadcrumb";

        private IPageProvider pageProvider;
        private INavigation navigation;
        private IHtmlHelper htmlHelper;
        private readonly IScopeService scopeService;

        [HtmlAttributeName(BcAttributeName)]
        public string BreadCrumbStyle { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public BreadCrumbHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor, IScopeService scopeService)
             : base(httpContextAccessor, scopeService)
        {
            pageProvider = container.Resolve<IPageProvider>();
            htmlHelper = container.Resolve<IHtmlHelper>();
            navigation = container.Resolve<INavigation>();
            this.scopeService = scopeService;


        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(BreadCrumbStyle))
            {
                output.Content.SetHtmlContent("BreadCrumb style (sd-breadcrumb) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);
            List<Page> pages = navigation.GetBreadCrumbs(scopeService.PageContext.CurrentPageId);

            var htmlContent = htmlHelper.Partial(string.Format(Globals.BreadCrumbStylePath, BreadCrumbStyle), pages);
            var contentResult = GetString(htmlContent);
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);

        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
