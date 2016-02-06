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
            pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(CurrentPage.Layout.Config);
            if (pageLayout.PlaceHolders != null && pageLayout.PlaceHolders.Count > 0)
            {
                string result = RenderContentItems(pageLayout.PlaceHolders, ModuleActionResults);
                output.Content.SetHtmlContent(result);
            }
        }

        private string RenderContentItems(List<PlaceHolder> placeHolders, Dictionary<string, string> moduleActionResults)
        {
            StringBuilder sb = new StringBuilder();
            if (placeHolders != null)
            {
                foreach (var placeHolder in placeHolders)
                {
                    if (placeHolder != null)
                    {
                        ControlData controlData = new ControlData();
                        IHtmlContent htmlContent;
                        string moduleResult = null, contentResult = null;
                        var layoutType = placeHolder.Type.ToLower();
                        ViewContext.ViewData["isEditMode"] = false;

                        if (moduleActionResults.ContainsKey(placeHolder.Id.ToString()))
                        {
                            //Modules                            
                            if (moduleActionResults.TryGetValue(placeHolder.Id.ToString(), out moduleResult))
                            {
                                //sb.Append(moduleResult);
                            }
                        }

                        if (CurrentPage.PageContent.Any(pc => pc.ContainerId == placeHolder.Id))
                        {
                            //Page contents
                            var pageContents = CurrentPage.PageContent
                                .Where(pc => pc.ContainerId == placeHolder.Id)
                                .OrderBy(pc => pc.SortOrder);
                            foreach (var pageContent in pageContents)
                            {
                                dynamic typeInfo = JsonConvert.DeserializeObject<dynamic>(pageContent.TypeInfo);
                                string typeName = (string)typeInfo.type;
                                ViewContext.ViewData["contentType"] = typeName;
                                htmlContent = htmlHelper.Partial(string.Format("~/Views/Shared/ContentTypes/View/{0}.cshtml", typeName), pageContent);
                                contentResult = GetString(htmlContent);
                            }
                        }
                        //TODO: Sort order (1. within module, 2.within contents, 3. mix of modules, contents and placeholders) will not work, later it should be implemented
                        //Repeaters (container/colum/row)
                        controlData.HtmlResult+= (!string.IsNullOrEmpty(moduleResult))?moduleResult: "";
                        controlData.HtmlResult += (!string.IsNullOrEmpty(contentResult))? contentResult : "";
                        controlData.HtmlResult += RenderContentItems(placeHolder.PlaceHolders, moduleActionResults);
                        htmlContent = htmlHelper.Partial(string.Format("~/Views/Shared/LayoutTypes/{0}.cshtml", layoutType), controlData);
                        sb.Append(GetString(htmlContent));
                        
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
