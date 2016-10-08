using System.Collections.Generic;
using Deviser.Core.Common.DomainTypes;
using System;

namespace Deviser.Core.Library.Sites
{
    public interface IContentManager
    {
        PageContent Get(Guid pageContentId);
        List<PageContent> Get(Guid pageId, string cultureCode);
        PageContent AddOrUpdatePageContent(PageContent pageContent);
        void AddOrUpdatePageContents(List<PageContent> contents);
        bool DeletePageContent(Guid id);
        void UpdateContentPermission(PageContent pageContent);
        bool HasViewPermission(PageContent pageContent);
        bool HasEditPermission(PageContent pageContent);
    }
}