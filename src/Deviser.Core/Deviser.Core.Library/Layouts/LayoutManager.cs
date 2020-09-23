using AutoMapper;
using Deviser.Core.Common;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Deviser.Core.Library.Layouts
{
    public class LayoutManager : ILayoutManager
    {
        //Logger
        private readonly ILogger<LayoutManager> _logger;
        private readonly ILayoutRepository _layoutRepository;
        private readonly IMapper _mapper;
        private readonly IPageRepository _pageRepository;
        private readonly IPageContentRepository _pageContentRepository;

        public LayoutManager(ILogger<LayoutManager> logger,
            ILayoutRepository layoutRepository,
            IMapper mapper,
            IPageRepository pageRepository,
            IPageContentRepository pageContentRepository)
        {
            _logger = logger;
            _layoutRepository = layoutRepository;
            _mapper = mapper;
            _pageRepository = pageRepository;
            _pageContentRepository = pageContentRepository;
        }


        public List<PageLayout> GetPageLayouts()
        {
            var resultLayouts = _layoutRepository.GetLayouts();
            var result = new List<PageLayout>();
            foreach (var layout in resultLayouts)
            {
                if (layout != null && !string.IsNullOrEmpty(layout.Config))
                    result.Add(ConvertToPageLayout(layout));
            }
            return result;
        }

        public List<Layout> GetDeletedLayouts()
        {
            var result = _layoutRepository.GetDeletedLayouts();
            return result;
        }

        public PageLayout GetPageLayout(Guid layoutId)
        {
            var resultLayout = _layoutRepository.GetLayout(layoutId);
            var result = ConvertToPageLayout(resultLayout);
            return result;
        }

        public PageLayout CreatePageLayout(PageLayout pageLayout)
        {
            var layout = ConvertToLayout(pageLayout);
            var resultLayout = _layoutRepository.CreateLayout(layout);
            UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
            var result = ConvertToPageLayout(resultLayout);
            return result;
        }

        public PageLayout UpdatePageLayout(PageLayout pageLayout)
        {

            var layout = ConvertToLayout(pageLayout);
            var resultLayout = _layoutRepository.UpdateLayout(layout);
            UpdatePageLayout(pageLayout.PageId, resultLayout.Id);
            var result = ConvertToPageLayout(resultLayout);
            return result;
        }


        private Layout ConvertToLayout(PageLayout pageLayout)
        {
            var layout = _mapper.Map<Layout>(pageLayout);
            layout.Config = JsonConvert.SerializeObject(pageLayout.PlaceHolders); //JsonConvert.DeserializeObject<List<ContentItem>>(Model.Layout.Config, new ContentItemConverter());
            return layout;
        }

        private PageLayout ConvertToPageLayout(Layout layout)
        {
            var pageLayout = _mapper.Map<PageLayout>(layout);
            pageLayout.PlaceHolders = JsonConvert.DeserializeObject<List<PlaceHolder>>(layout.Config);
            return pageLayout;

        }

        private void UpdatePageLayout(Guid pageId, Guid layoutId)
        {
            var page = _pageRepository.GetPage(pageId);
            page.LayoutId = layoutId;
            page.Layout = null;
            _pageRepository.UpdatePageActiveAndLayout(page);
        }

        public Layout UpdateLayout(Layout layout)
        {
            layout.IsActive = true;
            var result = _layoutRepository.UpdateLayout(layout);
            return result;
        }
    }
}
