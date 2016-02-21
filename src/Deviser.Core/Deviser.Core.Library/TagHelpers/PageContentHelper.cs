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
        public Dictionary<string, List<ContentResult>> ModuleActionResults { get; set; }

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

        private string RenderContentItems(List<PlaceHolder> placeHolders, Dictionary<string, List<ContentResult>> moduleActionResults)
        {
            StringBuilder sb = new StringBuilder();
            if (placeHolders != null)
            {
                foreach (var placeHolder in placeHolders)
                {
                    if (placeHolder != null)
                    {
                        //ControlData controlData = new ControlData();
                        string currentResult="";
                        List<ContentResult> currentResults = new List<ContentResult>();
                        IHtmlContent htmlContent;                        
                        List<ContentResult> moduleResult = null;
                        var layoutType = placeHolder.Type.ToLower();
                        ViewContext.ViewData["isEditMode"] = false;

                        if (moduleActionResults.ContainsKey(placeHolder.Id.ToString()))
                        {
                            //Modules                            
                            if (moduleActionResults.TryGetValue(placeHolder.Id.ToString(), out moduleResult))
                            {
                                //sb.Append(moduleResult);
                                currentResults.AddRange(moduleResult);
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
                                var contentResult = GetString(htmlContent);
                                currentResults.Add(new ContentResult
                                {
                                    Result = contentResult,
                                    SortOrder = pageContent.SortOrder
                                });
                            }
                        }


                        var placeHolderResult = RenderContentItems(placeHolder.PlaceHolders, moduleActionResults);
                        currentResults.Add(new ContentResult
                        {
                            Result = placeHolderResult,
                            SortOrder = placeHolder.Index
                        });

                        var sortedResult = currentResults.OrderBy(r => r.SortOrder).ToList();
                        foreach(var contentResult in sortedResult)
                        {
                            currentResult += contentResult.Result;
                        }

                        //TODO: Sort order (1. within module, 2.within contents, 3. mix of modules, contents and placeholders) will not work, later it should be implemented
                        //Repeaters (container/colum/row)
                        //currentResult += (!string.IsNullOrEmpty(moduleResult.Result))?moduleResult.Result: "";
                        //currentResult += (!string.IsNullOrEmpty(contentResult))? contentResult : "";
                        //currentResult += RenderContentItems(placeHolder.PlaceHolders, moduleActionResults);



                        htmlContent = htmlHelper.Partial(string.Format("~/Views/Shared/LayoutTypes/{0}.cshtml", layoutType), currentResult);
                        var layoutResult = GetString(htmlContent);
                        sb.Append(layoutResult);
                        
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
