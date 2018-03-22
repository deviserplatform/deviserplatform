using System;
using System.Collections.Generic;
using AutoMapper;
using Deviser.Core.Data.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Autofac;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;


namespace Deviser.Core.Library.Layouts
{
    public class LayoutManager : ILayoutManager
    {
        //Logger
        private readonly ILogger<LayoutManager> _logger;
        private readonly ILayoutRepository _layoutRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IPageContentRepository _pageContentRepository;

        public LayoutManager(ILifetimeScope container)
        {
            _logger = container.Resolve<ILogger<LayoutManager>>();
            _layoutRepository = container.Resolve<ILayoutRepository>();
            _pageRepository = container.Resolve<IPageRepository>();
            _pageContentRepository = container.Resolve<IPageContentRepository>();
        }


        public List<PageLayout> GetPageLayouts()
        {
            try
            {
                var resultLayouts = _layoutRepository.GetLayouts();
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
                _logger.LogError(string.Format("Error occured while getting all page layouts"), ex);
            }
            return null;
        }

        public List<Layout> GetDeletedLayouts()
        {
            try
            {
                var result = _layoutRepository.GetDeletedLayouts();
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting deleted layouts"), ex);
            }
            return null;
        }

        public PageLayout GetPageLayout(Guid layoutId)
        {
            try
            {
                var resultLayout = _layoutRepository.GetLayout(layoutId);
                PageLayout result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while getting oage layout: {0}", layoutId), ex);
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
                var resultLayout = _layoutRepository.CreateLayout(layout);
                UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
                var result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while creating a page layout, LayoutName: ", pageLayout.Name), ex);
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
                var resultLayout = _layoutRepository.UpdateLayout(layout);
                UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
                var result = ConvertToPageLayout(resultLayout);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while updating page layout, LayoutName: ", pageLayout.Name), ex);
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
            var page = _pageRepository.GetPage(pageId, false);
            page.LayoutId = layoutId;
            page.Layout = null;
            _pageRepository.UpdatePage(page);
        }

        private void DeleteModulesAndContent(PageLayout pageLayout)
        {
            //When page layout is being copied, all modules and contents should be deleted.
            var pageModules = _pageRepository.GetPageModules(pageLayout.PageId);
            var pageContents = _pageContentRepository.Get(pageLayout.PageId, Globals.FallbackLanguage);
            if (pageModules != null && pageModules.Count > 0)
            {
                foreach (var pageModule in pageModules)
                {
                    pageModule.IsDeleted = true;
                    _pageRepository.UpdatePageModule(pageModule);
                }
            }

            if (pageContents != null && pageContents.Count > 0)
            {
                foreach (var content in pageContents)
                {
                    content.IsDeleted = true;
                    _pageContentRepository.Update(content);
                }
            }
        }

        public Layout UpdateLayout(Layout layout)
        {
            try
            {
                layout.IsDeleted = false;
                var result = _layoutRepository.UpdateLayout(layout);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while restoring the layout"), ex);
            }
            return null;
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
