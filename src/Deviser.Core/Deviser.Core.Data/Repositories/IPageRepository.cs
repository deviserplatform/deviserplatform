using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IPageRepository //: IRepositoryBase
    {
        Page GetPageTree(bool isActiveOnly = false);

        Page GetPageTree(Guid pageId, bool isActiveOnly = false);
        //IList<Page> GetPages();
        IList<Page> GetPagesAndTranslations();
        IList<Page> GetDeletedPages();
        Page GetPage(Guid pageId);
        Page GetPageAndPagePermissions(Guid pageId);
        Page GetPageAndPageTranslations(Guid pageId);
        Page GetPageAndDependencies(Guid pageId, bool includeChild = true);
        IList<Page> GetPagesFlat(bool refreshCache = false);
        IList<PageType> GetPageTypes(bool refreshCache = false);
        Page CreatePage(Page dbPage);
        Page UpdatePageActiveAndLayout(Page page);
        Page UpdatePageAndPermissions(Page dbPage);
        Page UpdatePageTree(Page dbPage);
        Page RestorePage(Guid id);
        IList<PageTranslation> GetPageTranslations(string locale);
        PageTranslation GetPageTranslation(string url);
        IList<PageModule> GetPageModules(Guid pageId);
        IList<PageModule> GetDeletedPageModules();
        PageModule GetPageModule(Guid pageModuleId);
        //PageModule GetPageModuleByContainer(Guid containerId);
        PageModule CreatePageModule(PageModule dbPageModule);
        PageModule UpdatePageModule(PageModule dbPageModule);
        void AddOrUpdatePageModules(IList<PageModule> dbPageModules);
        PageModule UpdateModulePermission(PageModule dbPageModule);
        IList<PagePermission> AddPagePermissions(IList<PagePermission> dbPagePermissions);
        IList<ModulePermission> AddModulePermissions(IList<ModulePermission> dbModulePermissions);
        PageModule RestorePageModule(Guid id);
        bool DeletePageModule(Guid id);
        bool DeletePage(Guid id);
        bool DraftPage(Guid id);
        bool PublishPage(Guid id);
    }
}