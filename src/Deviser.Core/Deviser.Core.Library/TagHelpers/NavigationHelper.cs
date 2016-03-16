using Autofac;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
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
    [HtmlTargetElement("*", Attributes = NavAttributeName)]
    [HtmlTargetElement("*", Attributes = ShowAdminAttributeName)]
    public class NavigationHelper : DeviserTagHelper
    {
        private const string NavAttributeName = "sde-nav";
        private const string ShowAdminAttributeName = "sde-nav-showadmin";

        private IPageProvider pageProvider;
        private IHtmlHelper htmlHelper;

        [HtmlAttributeName(NavAttributeName)]
        public string MenuStyle { get; set; }

        [HtmlAttributeName(NavAttributeName)]
        public bool ShowAdmin { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public NavigationHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
            pageProvider = container.Resolve<IPageProvider>();
            this.htmlHelper = container.Resolve<IHtmlHelper>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(MenuStyle))
            {
                output.Content.SetHtmlContent("Navigation style (sde-nav) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);

            var root = pageProvider.GetPageTree();
            var pages = root.ChildPage.ToList();

            //Filter admin pages
            if (pages != null && !ShowAdmin)
                pages = pages.Where(p => !p.IsSystem).ToList();

            foreach (var page in pages)
            {
                FilterPage(page, ShowAdmin);
            }


            var htmlContent = htmlHelper.Partial(string.Format(Globals.MenuStylePath, MenuStyle), pages);
            var contentResult = GetString(htmlContent);
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);

        }

        private void FilterPage(Page page, bool isAdmin)
        {
            if (page != null)
            {
                if (page.Id == AppContext.CurrentPageId)
                {
                    page.IsActive = true;
                }

                //Filter admin pages
                if (page.ChildPage != null && !isAdmin)
                    page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                    {
                        if (child.Id == AppContext.CurrentPageId)
                        {
                            page.IsBreadCrumb = true;
                        }

                        FilterPage(child, isAdmin);
                    }
                }
            }
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }
}
