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
using Deviser.Core.Common.Extensions;

using PageContent = Deviser.Core.Common.DomainTypes.PageContent;
using Deviser.Core.Data.Repositories;
//using Page = Deviser.Core.Data.Entities.Page;

namespace Deviser.Core.Library.TagHelpers
{
    [HtmlTargetElement("div", Attributes = PageAttributeName)]
    [HtmlTargetElement("div", Attributes = ModuleResultAttributeName)]
    public class PageContentHelper : TagHelper
    {
        private const string PageAttributeName = "sde-page";
        private const string ModuleResultAttributeName = "sde-module-results";

        private readonly IHtmlHelper _htmlHelper;
        private readonly IScopeService _scopeService;
        private readonly IPropertyRepository _propertyRepository;

        public PageContentHelper(IHtmlHelper htmlHelper, IHtmlGenerator generator, 
            IScopeService scopeService, IPropertyRepository propertyRepository)
        {
            Generator = generator;
            _htmlHelper = htmlHelper;
            _scopeService = scopeService;
            _propertyRepository = propertyRepository;
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
                return _scopeService.PageContext.CurrentCulture;
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

            ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);

            PageLayout pageLayout = new PageLayout();
            pageLayout.Name = CurrentPage.Layout.Name;

            var properties = _propertyRepository.GetProperties();
            pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(CurrentPage.Layout.Config);

            GetPropertyValue(pageLayout.PlaceHolders, properties);

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
                HtmlContentBuilder result = RenderContentItems(pageLayout.PlaceHolders);
                if (ModuleActionResults.Count > 0)
                {
                    //One or more modules are not added in containers
                    foreach(var moduleActionResult in ModuleActionResults)
                    {
                        if(moduleActionResult.Value!=null && moduleActionResult.Value.Count > 0)
                        {
                            foreach(var contentResult in moduleActionResult.Value)
                            {
                                //result += contentResult.HtmlResult;
                                result.AppendHtml(contentResult.HtmlResult);
                            }
                        }
                    }
                }
                output.Content.SetHtmlContent(result);

                //Release all resources
                pageLayout.RegisterForDispose(ViewContext.HttpContext);
                PageContents.GetEnumerator().RegisterForDispose(ViewContext.HttpContext);                
                ModuleActionResults.GetEnumerator().RegisterForDispose(ViewContext.HttpContext);
                pageLayout.RegisterForDispose(ViewContext.HttpContext);
            }
        }

        private void GetPropertyValue(List<PlaceHolder> placeHolders, List<Property> properties)
        {
            foreach(var placeHolder in placeHolders)
            {
                if(placeHolder!=null)
                {
                    if(placeHolder.Properties!=null && placeHolder.Properties.Count > 0)
                    {
                        foreach(var prop in placeHolder.Properties)
                        {
                            if (prop != null)
                            {
                                var masterProp = properties.FirstOrDefault(p => p.Id == prop.Id);
                                prop.OptionList = masterProp.OptionList;
                            }
                        }
                    }

                    if(placeHolder.PlaceHolders != null && placeHolder.PlaceHolders.Count > 0)
                    {
                        GetPropertyValue(placeHolder.PlaceHolders, properties);
                    }
                }
            }
        }

        private HtmlContentBuilder RenderContentItems(List<PlaceHolder> placeHolders)
        {
            //StringBuilder sb = new StringBuilder();
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();
            if (placeHolders != null)
            {
                foreach (var placeHolder in placeHolders)
                {
                    if (placeHolder != null)
                    {
                        //string currentResult = "";
                        var htmlCurrentResult = new HtmlContentBuilder();

                        List<ContentResult> currentResults = new List<ContentResult>();
                        IHtmlContent htmlContent;
                        List<ContentResult> moduleResult = null;
                        var layoutType = placeHolder.Type;
                        ViewContext.ViewData["isEditMode"] = false;

                        if (ModuleActionResults.ContainsKey(placeHolder.Id.ToString()))
                        {
                            //Modules                            
                            if (ModuleActionResults.TryGetValue(placeHolder.Id.ToString(), out moduleResult))
                            {
                                //sb.Append(moduleResult);
                                ModuleActionResults.Remove(placeHolder.Id.ToString());
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
                                dynamic content = (contentTranslation != null) ? SDJsonConvert.DeserializeObject<dynamic>(contentTranslation.ContentData) : null;

                                if (content != null)
                                {
                                    var dynamicContent = new DynamicContent
                                    {
                                        PageContent = pageContent,
                                        Content = content
                                    };

                                    string contentTypesViewPath = string.Format(Globals.ContentTypesViewPath, _scopeService.PageContext.SelectedTheme, typeName);

                                    htmlContent = _htmlHelper.Partial(contentTypesViewPath, dynamicContent);
                                    //var contentResult = GetString(htmlContent);
                                    currentResults.Add(new ContentResult
                                    {
                                        //Result = contentResult,
                                        HtmlResult = htmlContent,
                                        SortOrder = pageContent.SortOrder
                                    });
                                }
                            }
                        }


                        var placeHolderResult = RenderContentItems(placeHolder.PlaceHolders);
                        currentResults.Add(new ContentResult
                        {
                            //Result = placeHolderResult,
                            HtmlResult = placeHolderResult,
                            SortOrder = placeHolder.SortOrder
                        });

                        var sortedResult = currentResults.OrderBy(r => r.SortOrder).ToList();
                        foreach (var contentResult in sortedResult)
                        {                            
                            //currentResult += contentResult.Result;
                            htmlCurrentResult.AppendHtml(contentResult.HtmlResult);
                        }

                        //TODO: Sort order (1. within module, 2.within contents, 3. mix of modules, contents and placeholders) will not work, later it should be implemented
                        //Repeaters (container/colum/row)
                        //currentResult += (!string.IsNullOrEmpty(moduleResult.Result))?moduleResult.Result: "";
                        //currentResult += (!string.IsNullOrEmpty(contentResult))? contentResult : "";
                        //currentResult += RenderContentItems(placeHolder.PlaceHolders, moduleActionResults);

                        var layoutContent = new LayoutContent
                        {
                            PlaceHolder = placeHolder,
                            ContentResult = htmlCurrentResult
                        };

                        htmlContent = _htmlHelper.Partial(string.Format(Globals.LayoutTypesPath, _scopeService.PageContext.SelectedTheme, layoutType), layoutContent);
                        //var layoutResult = GetString(htmlContent);
                        contentBuilder.AppendHtml(htmlContent);
                        //sb.Append(layoutResult);

                    }
                }
            }
            //return sb.ToString();
            return contentBuilder;
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
