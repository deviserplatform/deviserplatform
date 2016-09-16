using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using PageModule = Deviser.Core.Data.Entities.PageModule;
using Layout = Deviser.Core.Data.Entities.Layout;

namespace Deviser.Core.Library.Layouts
{
    public class LayoutManager : ILayoutManager
    {
        //Logger
        private readonly ILogger<LayoutManager> logger;

        ILayoutProvider layoutProvider;
        IPageProvider pageProvider;
        IPageContentProvider pageContentProvider;

        public LayoutManager(ILifetimeScope container)
        {
            logger = container.Resolve<ILogger<LayoutManager>>();
            layoutProvider = container.Resolve<ILayoutProvider>();
            pageProvider = container.Resolve<IPageProvider>();
            pageContentProvider = container.Resolve<IPageContentProvider>();
        }


        public List<PageLayout> GetPageLayouts()
        {
            try
            {
                var resultLayouts = layoutProvider.GetLayouts();
                List<PageLayout> result = new List<PageLayout>();
                foreach (var layout in resultLayouts)
                {
                    if (layout != null && !string.IsNullOrEmpty(layout.Config))
                        result.Add(ConvertToPageLayout(layout));
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting all page layouts"), ex);
            }
            return null;
        }

        public PageLayout GetPageLayout(Guid layoutId)
        {
            try
            {
                var resultLayout = layoutProvider.GetLayout(layoutId);
                PageLayout result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while getting oage layout: {0}", layoutId), ex);
            }
            return null;
        }

        public PageLayout CreatePageLayout(PageLayout pageLayout)
        {
            try
            {
                //Not necessary, since layout and content has been seperated
                //if (pageLayout.IsChanged)
                //{
                //    DeleteModulesAndContent(pageLayout);
                //    CreateElement(pageLayout.ContentItems, pageLayout.PageId);
                //}
                var layout = ConvertToLayout(pageLayout);
                var resultLayout = layoutProvider.CreateLayout(layout);
                UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
                var result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: ", pageLayout.Name), ex);
            }
            return null;
        }

        public PageLayout UpdatePageLayout(PageLayout pageLayout)
        {
            try
            {
                //Not necessary, since layout and content has been seperated
                //if (pageLayout.IsChanged)
                //{
                //    DeleteModulesAndContent(pageLayout);
                //    CreateElement(pageLayout.ContentItems, pageLayout.PageId);
                //}                
                var layout = ConvertToLayout(pageLayout);
                var resultLayout = layoutProvider.UpdateLayout(layout);
                UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
                var result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", pageLayout.Name), ex);
            }
            return null;
        }


        private Layout ConvertToLayout(PageLayout pageLayout)
        {
            Layout layout = Mapper.Map<Layout>(pageLayout);
            layout.Config = JsonConvert.SerializeObject(pageLayout.PlaceHolders); //JsonConvert.DeserializeObject<List<ContentItem>>(Model.Layout.Config, new ContentItemConverter());
            return layout;
        }

        private PageLayout ConvertToPageLayout(Layout layout)
        {
            var pageLayout = Mapper.Map<PageLayout>(layout);
            pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(layout.Config);
            return pageLayout;

        }

        private void UpdatePageLayout(Guid pageId, Guid layoutId)
        {
            var page = pageProvider.GetPage(pageId);
            page.LayoutId = layoutId;
            page.Layout = null;
            pageProvider.UpdatePage(page);
        }

        private void DeleteModulesAndContent(PageLayout pageLayout)
        {
            //When page layout is being copied, all modules and contents should be deleted.
            var pageModules = pageProvider.GetPageModules(pageLayout.PageId);
            var pageContents = pageContentProvider.Get(pageLayout.PageId, Globals.FallbackLanguage);
            if (pageModules != null && pageModules.Count > 0)
            {
                foreach (var pageModule in pageModules)
                {
                    pageModule.IsDeleted = true;
                    pageProvider.UpdatePageModule(pageModule);
                }
            }

            if (pageContents != null && pageContents.Count > 0)
            {
                foreach (var content in pageContents)
                {
                    content.IsDeleted = true;
                    pageContentProvider.Update(content);
                }
            }
        }

        //private void CreateElement(List<PlaceHolder> placeHolders, Guid pageId)
        //{
        //    if (placeHolders != null && placeHolders.Count > 0)
        //    {
        //        foreach (var placeHolder in placeHolders)
        //        {
        //            if (placeHolder.Type == "text")
        //            {
        //                var translations = new List<PageContentTranslation>();
        //                translations.Add(new PageContentTranslation
        //                {
        //                    CultureCode = Globals.FallbackLanguage
        //                });

        //                Data.Entities.PageContent pageContent = new Data.Entities.PageContent
        //                {
        //                    PageId = pageId,
        //                    ContainerId = placeHolder.Id,
        //                    PageContentTranslation = translations
        //                    //CultureCode = Globals.FallbackLanguage
        //                };
        //                pageContentProvider.Create(pageContent);

        //            }
        //            else if (placeHolder.Type == "module")
        //            {
        //                PageModule pageModule = new PageModule
        //                {
        //                    PageId = pageId,
        //                    ModuleId = placeHolder.Module.Id,
        //                    ContainerId = placeHolder.Id
        //                };
        //                pageProvider.CreatePageModule(pageModule);
        //            }

        //            if (placeHolder.PlaceHolders != null)
        //            {
        //                CreateElement(placeHolder.PlaceHolders, pageId);
        //            }
        //        }
        //    }
        //}
    }
}
