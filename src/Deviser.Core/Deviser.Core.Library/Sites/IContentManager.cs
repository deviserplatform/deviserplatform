using Deviser.Core.Common.DomainTypes;
using System;
using System.Collections.Generic;

namespace Deviser.Core.Library.Sites
{
    public interface IContentManager
    {
        PageContent Get(Guid pageContentId);
        List<PageContent> Get(Guid pageId, string cultureCode);
        List<PageContent> GetDeletedPageContents();
        PageContent RestorePageContent(Guid id);
        PageContent AddOrUpdatePageContent(PageContent pageContent);
        void AddOrUpdatePageContents(List<PageContent> contents);
        bool RemovePageContent(Guid id);
        bool DeletePageContent(Guid id);
        PageContent UpdateContentPermission(PageContent pageContent);
        bool HasViewPermission(PageContent pageContent, bool isForCurrentRequest = false);
        bool HasEditPermission(PageContent pageContent, bool isForCurrentRequest = false);
    }
}