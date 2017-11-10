using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using AutoMapper;
using Deviser.Core.Library.Services;
using System.Globalization;

using PageContent = Deviser.Core.Common.DomainTypes.PageContent;
//using Page = Deviser.Core.Data.Entities.Page;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("div", Attributes = PageAttributeName)]
    [HtmlTargetElement("div", Attributes = ModuleResultAttributeName)]
    public class PageContentHelper : TagHelper
    {
        private const string PageAttributeName = "sde-page";
        private const string ModuleResultAttributeName = "sde-module-results";

        private IHtmlHelper htmlHelper;
        private readonly IScopeService scopeService;

        public PageContentHelper(IHtmlHelper htmlHelper, IHtmlGenerator generator, IScopeService scopeService)
        {
            Generator = generator;
            this.htmlHelper = htmlHelper;
            this.scopeService = scopeService;
        }

        [HtmlAttributeName(PageAttributeName)]
        public Page CurrentPage { get; set; }

        [HtmlAttributeName(ModuleResultAttributeName)]
        public Dictionary<string, List<ContentResult>> ModuleActionResults { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected ICollection<PageContent> PageContents { get; set; }

        protected CultureInfo CurrentCulture
        {
            get
            {
                return scopeService.PageContext.CurrentCulture;
            }
        }

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

            if (CurrentPage.Layout == null)
            {
                output.Content.SetHtmlContent("Layout is not configured, choose a layout");
                return;
            }

            ((HtmlHelper)htmlHelper).Contextualize(ViewContext);

            PageLayout pageLayout = new PageLayout();
            pageLayout.Name = CurrentPage.Layout.Name;
            pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(CurrentPage.Layout.Config);
            PageContents = Mapper.Map<ICollection<PageContent>>(CurrentPage.PageContent);

            //Copy property options from master data
            if (PageContents != null && PageContents.Count > 0)
            {
                foreach (var pageContent in PageContents)
                {
                    if (pageContent.ContentType.Properties != null && pageContent.ContentType.Properties.Count > 0)
                    {
                        foreach (var prop in pageContent.ContentType.Properties)
                        {
                            var propValue = pageContent.Properties.FirstOrDefault(p => p.Id == prop.Id);
                            if (propValue != null)
                            {
                                propValue.OptionListId = prop.OptionListId;
                                propValue.OptionList = prop.OptionList;
                            }
                        }
                    }
                }
            }

            if (pageLayout.PlaceHolders != null && pageLayout.PlaceHolders.Count > 0)
            {
                string result = RenderContentItems(pageLayout.PlaceHolders, ModuleActionResults);
                if (ModuleActionResults.Count > 0)
                {
                    //One or more modules are not added in containers
                    foreach(var moduleActionResult in ModuleActionResults)
                    {
                        if(moduleActionResult.Value!=null && moduleActionResult.Value.Count > 0)
                        {
                            foreach(var contentResult in moduleActionResult.Value)
                            {
                                result += contentResult.Result;
                            }
                        }
                    }
                }
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
                        string currentResult = "";
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
                                moduleActionResults.Remove(placeHolder.Id.ToString());
                                currentResults.AddRange(moduleResult);
                            }
                        }

                        if (PageContents.Any(pc => pc.ContainerId == placeHolder.Id))
                        {
                            //Page contents                            
                            var pageContents = PageContents
                                .Where(pc => pc.ContainerId == placeHolder.Id)
                                .OrderBy(pc => pc.SortOrder).ToList();
                            foreach (var pageContent in pageContents)
                            {
                                string typeName = pageContent.ContentType.Name;
                                var contentTranslation = pageContent.PageContentTranslation.FirstOrDefault(t => t.CultureCode.ToLower() == CurrentCulture.ToString().ToLower());
                                dynamic content = null;
                                if (pageContent.ContentType.ContentDataType.Name == "string")
                                {
                                    content = (contentTranslation != null) ? contentTranslation.ContentData : null;
                                }
                                else
                                {
                                    content = (contentTranslation != null) ? SDJsonConvert.DeserializeObject<dynamic>(contentTranslation.ContentData) : null;
                                }

                                var dynamicContent = new DynamicContent
                                {
                                    PageContent = pageContent,
                                    Content = content
                                };
                                string contentTypesViewPath = string.Format(Globals.ContentTypesViewPath, scopeService.PageContext.SelectedTheme, typeName);
                                htmlContent = htmlHelper.Partial(contentTypesViewPath, dynamicContent);
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
                            SortOrder = placeHolder.SortOrder
                        });

                        var sortedResult = currentResults.OrderBy(r => r.SortOrder).ToList();
                        foreach (var contentResult in sortedResult)
                        {
                            currentResult += contentResult.Result;
                        }

                        //TODO: Sort order (1. within module, 2.within contents, 3. mix of modules, contents and placeholders) will not work, later it should be implemented
                        //Repeaters (container/colum/row)
                        //currentResult += (!string.IsNullOrEmpty(moduleResult.Result))?moduleResult.Result: "";
                        //currentResult += (!string.IsNullOrEmpty(contentResult))? contentResult : "";
                        //currentResult += RenderContentItems(placeHolder.PlaceHolders, moduleActionResults);

                        var layoutContent = new LayoutContent
                        {
                            PlaceHolder = placeHolder,
                            ContentResult = currentResult
                        };

                        htmlContent = htmlHelper.Partial(string.Format(Globals.LayoutTypesPath, scopeService.PageContext.SelectedTheme, layoutType), layoutContent);
                        var layoutResult = GetString(htmlContent);
                        sb.Append(layoutResult);

                    }
                }
            }
            return sb.ToString();
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
