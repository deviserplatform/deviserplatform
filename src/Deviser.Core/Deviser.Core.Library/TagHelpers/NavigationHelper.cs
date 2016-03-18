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
    public class NavigationHelper : DeviserTagHelper
    {
        private const string NavAttributeName = "sd-nav";
        private const string PageAttributeName = "sd-nav-page";
        private const string ParentAttributeName = "sd-nav-parent";

        private IPageProvider pageProvider;
        private INavigation navigation;
        private IHtmlHelper htmlHelper;

        [HtmlAttributeName(NavAttributeName)]
        public string MenuStyle { get; set; }

        [HtmlAttributeName(PageAttributeName)]
        public SystemPageFilter SystemFilter { get; set; }

        [HtmlAttributeName(ParentAttributeName)]
        public int parent { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public NavigationHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
            pageProvider = container.Resolve<IPageProvider>();
            this.htmlHelper = container.Resolve<IHtmlHelper>();
            this.navigation = container.Resolve<INavigation>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(MenuStyle))
            {
                output.Content.SetHtmlContent("Navigation style (sde-nav) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);
            Page root = null;
            if(parent>0)
            {
                root = navigation.GetPageTree(parent);
            }
            else
            {
                root = pageProvider.GetPageTree();
            }
            

            FilterPage(root, SystemFilter);

            var htmlContent = htmlHelper.Partial(string.Format(Globals.MenuStylePath, MenuStyle), root);
            var contentResult = GetString(htmlContent);
            output.Content.SetHtmlContent(contentResult);
            //output.PostContent.Append("MenuStyle: " + MenuStyle);

        }

        private void FilterPage(Page page, SystemPageFilter systemFilter)
        {
            if (page != null)
            {
                if (page.Id == AppContext.CurrentPageId)
                {
                    page.IsActive = true;
                }

                //Page filter
                if (page.ChildPage != null)
                {
                    //system page filter
                    if(systemFilter == SystemPageFilter.PublicOnly)
                    {
                        page.ChildPage = page.ChildPage.Where(p => !p.IsSystem).ToList();
                    }
                    else if(systemFilter== SystemPageFilter.SystemOnly)
                    {
                        page.ChildPage = page.ChildPage.Where(p => p.IsSystem).ToList();
                    }
                }
                    

                if (page.ChildPage != null && page.ChildPage.Count > 0)
                {
                    foreach (var child in page.ChildPage)
                    {
                        if (child.Id == AppContext.CurrentPageId)
                        {
                            page.IsBreadCrumb = true;
                        }

                        FilterPage(child, systemFilter);
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

    public enum SystemPageFilter
    {
        All,
        PublicOnly,
        SystemOnly                
    }
}
