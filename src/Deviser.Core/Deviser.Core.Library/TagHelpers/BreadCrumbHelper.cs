using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = BcAttributeName)]
    public class BreadCrumbHelper : DeviserTagHelper
    {
        private const string BcAttributeName = "sd-breadcrumb";

        private IPageProvider pageProvider;
        private INavigation navigation;
        private IHtmlHelper htmlHelper;

        [HtmlAttributeName(BcAttributeName)]
        public string BreadCrumbStyle { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public BreadCrumbHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
            pageProvider = container.Resolve<IPageProvider>();
            this.htmlHelper = container.Resolve<IHtmlHelper>();
            this.navigation = container.Resolve<INavigation>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(BreadCrumbStyle))
            {
                output.Content.SetHtmlContent("BreadCrumb style (sd-breadcrumb) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);
            List<Page> pages = navigation.GetBreadCrumbs(AppContext.CurrentPageId);

            var htmlContent = htmlHelper.Partial(string.Format(Globals.BreadCrumbStylePath, BreadCrumbStyle), pages);
            var contentResult = GetString(htmlContent);
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);

        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }
}
