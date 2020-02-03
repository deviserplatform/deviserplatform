﻿using AutoMapper;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Extensions;
using Deviser.Core.Library.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
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

        private readonly IHtmlHelper _htmlHelper;
        private readonly IMapper _mapper;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IScopeService _scopeService;

        public PageContentHelper(IHtmlHelper htmlHelper,
            IMapper mapper,
            IHtmlGenerator generator,
            IPropertyRepository propertyRepository,
            IScopeService scopeService)
        {
            Generator = generator;
            _htmlHelper = htmlHelper;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
            _scopeService = scopeService;
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

            try
            {
                ((HtmlHelper)_htmlHelper).Contextualize(ViewContext);

                PageLayout pageLayout = new PageLayout();
                pageLayout.Name = CurrentPage.Layout.Name;

                var properties = _propertyRepository.GetProperties();
                pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(CurrentPage.Layout.Config);

                GetPropertyValue(pageLayout.PlaceHolders, properties);

                PageContents = _mapper.Map<ICollection<PageContent>>(CurrentPage.PageContent);

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
                        foreach (var moduleActionResult in ModuleActionResults)
                        {
                            if (moduleActionResult.Value != null && moduleActionResult.Value.Count > 0)
                            {
                                foreach (var contentResult in moduleActionResult.Value)
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
            catch (Exception ex)
            {
                throw ex;
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
                    var result = RenderContentItem(placeHolder);
                    contentBuilder.AppendHtml(result);
                }
            }
            //return sb.ToString();
            return contentBuilder;
        }

        private IHtmlContent RenderContentItem(PlaceHolder placeHolder)
        {
            IHtmlContent htmlContentItems = null;
            if (placeHolder != null)
            {
                //string currentResult = "";
                var htmlCurrentResult = new HtmlContentBuilder();

                List<ContentResult> currentResults = new List<ContentResult>();
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

                            var htmlPageContent = _htmlHelper.Partial(contentTypesViewPath, dynamicContent);
                            //var contentResult = GetString(htmlContent);
                            currentResults.Add(new ContentResult
                            {
                                //Result = contentResult,
                                HtmlResult = htmlPageContent,
                                SortOrder = pageContent.SortOrder
                            });
                        }
                    }
                }

                //var placeHolderResult = RenderContentItems(placeHolder.PlaceHolders);
                //currentResults.Add(new ContentResult
                //{                    
                //    HtmlResult = placeHolderResult,
                //    SortOrder = placeHolder.SortOrder
                //});

                if(placeHolder.PlaceHolders!=null && placeHolder.PlaceHolders.Count > 0)
                {
                    foreach (var childPlaceHolder in placeHolder.PlaceHolders)
                    {
                        var placeHolderResult = RenderContentItem(childPlaceHolder);
                        currentResults.Add(new ContentResult
                        {
                            HtmlResult = placeHolderResult,
                            SortOrder = childPlaceHolder.SortOrder
                        });
                    }
                }


                var sortedResult = currentResults.OrderBy(r => r.SortOrder).ToList();
                foreach (var contentResult in sortedResult)
                {
                    //currentResult += contentResult.Result;
                    htmlCurrentResult.AppendHtml(contentResult.HtmlResult);
                }

                var layoutContent = new LayoutContent
                {
                    PlaceHolder = placeHolder,
                    ContentResult = htmlCurrentResult
                };

                htmlContentItems = _htmlHelper.Partial(string.Format(Globals.LayoutTypesPath, _scopeService.PageContext.SelectedTheme, layoutType), layoutContent);
                //var layoutResult = GetString(htmlContent);                
                //sb.Append(layoutResult);

            }

            return htmlContentItems;
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
