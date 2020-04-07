using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library;
using Deviser.Core.Library.Layouts;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Multilingual;
using Deviser.Core.Library.Sites;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace Deviser.Modules.PageManagement.Services
{
    public class PageManagementAdminService : IAdminTreeService<Page> //: IAdminFormService<RecycleItem>
    {
        private readonly ILogger<PageManagementAdminService> _logger;
        private readonly IPageRepository _pageRepository;
        private readonly INavigation _navigation;

        public PageManagementAdminService(ILogger<PageManagementAdminService> logger,
            IPageRepository pageRepository,
            INavigation navigation)
        {
            _logger = logger;
            _pageRepository = pageRepository;
            _navigation = navigation;
        }

        public async Task<Page> GetTree()
        {
            var pageTree = _pageRepository.GetPageTree();
            return await Task.FromResult(pageTree);
        }

        public async Task<Page> GetItem(string itemId)
        {
            var result = _navigation.GetPageAndDependencies(Guid.Parse(itemId));
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Page>> CreateItem(Page item)
        {
            var pageResult = _navigation.CreatePage(item);
            var result = new FormResult<Page>(pageResult);
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Page>> UpdateItem(Page item)
        {
            var pageResult = _navigation.UpdateSinglePage(item);
            var result = new FormResult<Page>(pageResult);
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Page>> UpdateTree(Page item)
        {
            var pageResult = _navigation.UpdatePageTree(item);
            var result = new FormResult<Page>(pageResult);
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<Page>> DeleteItem(string itemId)
        {

            Page page = _pageRepository.GetPage(Guid.Parse(itemId));
            if (page != null)
            {
                page.IsDeleted = true;
                var resultPage = _pageRepository.UpdatePage(page);
                if (resultPage != null)
                {
                    var result = new FormResult<Page>(resultPage);
                    return await Task.FromResult(result);
                }
            }

            return new FormResult<Page>()
            {
                IsSucceeded = false,
                ErrorMessage = "Unable to delete page"
            };
            
        }
    }
}
