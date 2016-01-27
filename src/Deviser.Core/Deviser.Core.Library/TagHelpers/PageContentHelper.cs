using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("div", Attributes = PageAttributeName)]
    [HtmlTargetElement("div", Attributes = ModuleResultAttributeName)]
    public class PageContentHelper : TagHelper
    {
        private const string PageAttributeName = "sde-page";
        private const string ModuleResultAttributeName = "sde-module-results";

        IHtmlHelper htmlHelper;

        public PageContentHelper(IHtmlHelper htmlHelper, IHtmlGenerator generator)
        {
            Generator = generator;
            this.htmlHelper = htmlHelper;
        }

        [HtmlAttributeName(PageAttributeName)]
        public Page CurrentPage { get; set; }

        [HtmlAttributeName(ModuleResultAttributeName)]
        public Dictionary<string, string> ModuleActionResults { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (CurrentPage == null)
            {
                output.Content.SetHtmlContent("Page object (sde-page) cannot be null");
                return;
            }

            if (ModuleActionResults == null)
            {
                output.Content.SetHtmlContent("Module action results  (sde-module-results) cannot be null");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);

            PageLayout pageLayout = new PageLayout();
            pageLayout.Name = CurrentPage.Layout.Name;
            pageLayout.ContentItems = JsonConvert.DeserializeObject<List<ContentItem>>(CurrentPage.Layout.Config);
            if (pageLayout.ContentItems != null && pageLayout.ContentItems.Count > 0)
            {
                string result = RenderContentItems(pageLayout.ContentItems, ModuleActionResults);
                output.Content.SetHtmlContent(result);
            }
        }

        private string RenderContentItems(List<ContentItem> contentItems, Dictionary<string, string> moduleActionResults)
        {
            StringBuilder sb = new StringBuilder();
            if (contentItems != null)
            {
                foreach (var contentItem in contentItems)
                {
                    if (contentItem != null)
                    {                        
                        ControlData controlData = new ControlData();
                        var contentType = contentItem.Type.ToLower();                        
                        
                        if (contentType == "module" && moduleActionResults.ContainsKey(contentItem.Id.ToString()))
                        {
                            string moduleResult;
                            if (moduleActionResults.TryGetValue(contentItem.Id.ToString(), out moduleResult))
                            {
                                sb.Append(moduleResult);
                            }
                        }
                        else
                        {
                            controlData.HtmlResult = RenderContentItems(contentItem.ContentItems, moduleActionResults);
                            var htmlContent = htmlHelper.Partial(string.Format("~/Views/Shared/ContentTypes/{0}.cshtml", contentType), controlData);
                            sb.Append(GetString(htmlContent));
                        }
                        //else
                        //{
                        //    //TODO: Get items and display it correspoinding placeholder
                        //    result = "This is text";
                        //    controlData.ContentItem = contentItem;
                        //    controlData.PageContents = CurrentPage.PageContent.Where(pc => pc.ContainerId == contentItem.Id).ToList();
                        //    controlData.HtmlResult = RenderContentItems(contentItem.ContentItems, moduleActionResults);
                        //    var htmlContent = htmlHelper.Partial(string.Format("~/Views/Shared/ContentTypes/{0}.cshtml", contentType), controlData);
                        //    sb.Append(GetString(htmlContent));
                        //}
                    }
                }
            }
            return sb.ToString();
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }
}
