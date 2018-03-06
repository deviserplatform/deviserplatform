using Autofac;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Library.Services;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("*", Attributes = NavAttributeName)]    
    public class NavigationHelper : DeviserTagHelper
    {
        private const string NavAttributeName = "sd-nav";
        private const string PageAttributeName = "sd-nav-page";
        private const string ParentAttributeName = "sd-nav-parent";

        private readonly IPageRepository _pageRepository;
        private readonly INavigation _navigation;
        private readonly IHtmlHelper _htmlHelper;
        private readonly IScopeService _scopeService;
        private readonly ILogger<NavigationHelper> _logger;

        [HtmlAttributeName(NavAttributeName)]
        public string MenuStyle { get; set; }

        [HtmlAttributeName(PageAttributeName)]
        public SystemPageFilter SystemFilter { get; set; }

        [HtmlAttributeName(ParentAttributeName)]
        public Guid ParentId { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public NavigationHelper(ILifetimeScope container, IHttpContextAccessor httpContextAccessor, IScopeService scopeService)
             : base(httpContextAccessor)
        {
            _pageRepository = container.Resolve<IPageRepository>();
            _htmlHelper = container.Resolve<IHtmlHelper>();
            _navigation = container.Resolve<INavigation>();
            _logger = container.Resolve<ILogger<NavigationHelper>>();
            this._scopeService = scopeService;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(MenuStyle))
            {
                output.Content.SetHtmlContent("Navigation style (sd-nav) cannot be null");
                return;
            }

            try
            {
                ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);
                var root = _navigation.GetPageTree(_scopeService.PageContext.CurrentPageId, SystemFilter, ParentId);
                
                var htmlContent = _htmlHelper.Partial(string.Format(Globals.MenuStylePath, _scopeService.PageContext.SelectedTheme, MenuStyle), root);
                var contentResult = GetString(htmlContent);
                output.Content.SetHtmlContent(contentResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("Page load exception has been occured", ex);
                output.Content.SetHtmlContent("Error occured, in menu");
            }
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }

   
}
