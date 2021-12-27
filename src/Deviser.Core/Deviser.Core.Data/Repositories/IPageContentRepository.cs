using System;
using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IPageContentRepository
    {
        PageContent Get(Guid pageContentId);
        List<PageContent> Get(Guid pageId, string cultureCode);
        List<PageContent> GetDeletedPageContents();
        PageContent RestorePageContent(Guid id);
        PageContentTranslation GetTranslation(Guid pageContentId);
        PageContentTranslation GetTranslations(Guid pageContentId, string cultureCode);
        PageContent Create(PageContent dbPageContent);
        PageContentTranslation CreateTranslation(PageContentTranslation contentTranslation);
        PageContent Update(PageContent content);
        void AddOrUpdate(List<PageContent> dbPageContents);
        PageContentTranslation UpdateTranslation(PageContentTranslation dbPageContentTranslation);
        List<ContentPermission> AddContentPermissions(List<ContentPermission> dbContentPermissions);
        PageContent UpdateContentPermission(PageContent dbPageContentSrc);
        PageContent SoftDeletePageContent(Guid id);
        bool DeletePageContentPermanent(Guid id);

    }
}