using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data.Entities;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataProviders
{
    public class PageContentProviderTest : TestBase
    {
        [Fact]
        public void GetSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageContentId = pageContents.First().Id;


            //Act
            var result = pageContentProvider.Get(pageContentId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageContentTranslation);
            Assert.NotNull(result.ContentType);
            Assert.NotNull(result.ContentType.ContentDataType);
            Assert.NotNull(result.ContentType.ContentTypeProperties);
            Assert.True(result.ContentType.ContentTypeProperties.Count > 0);
            Assert.NotNull(result.ContentPermissions);
            Assert.True(result.ContentPermissions.Count > 0);
            Assert.True(!string.IsNullOrEmpty(result.Properties));
            Assert.True(result.SortOrder > 0);
            Assert.NotEqual(result.ContainerId, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void GetOptionListFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageContentId = Guid.NewGuid();

            //Act
            var result = pageContentProvider.Get(pageContentId);

            //Assert
            Assert.Null(result);
        }

        //[Fact]
        //public void GetByContainerSuccess()
        //{
        //    //Arrange
        //    var pageContentProvider = new PageContentProvider(container);
        //    var pageContents = TestDataProvider.GetPageContents();
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    foreach (var item in pageContents)
        //    {
        //        pageContentProvider.Create(item);
        //    }
        //    var pageContentId = pageContents.First().ContainerId;


        //    //Act
        //    var result = pageContentProvider.GetByContainer(pageContentId);
        //    var resultItem = result.First();

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.True(result.Count > 0);
        //    Assert.NotNull(resultItem);
        //    Assert.NotEqual(resultItem.Id, Guid.Empty);
        //    Assert.NotNull(resultItem.PageContentTranslation);
        //    Assert.NotNull(resultItem.ContentType);
        //    Assert.NotNull(resultItem.ContentType.ContentDataType);
        //    Assert.NotNull(resultItem.ContentType.ContentTypeProperties);
        //    Assert.True(resultItem.ContentType.ContentTypeProperties.Count > 0);
        //    Assert.NotNull(resultItem.ContentPermissions);
        //    Assert.True(resultItem.ContentPermissions.Count > 0);
        //    Assert.True(!string.IsNullOrEmpty(resultItem.Properties));
        //    Assert.True(resultItem.SortOrder > 0);
        //    Assert.NotEqual(resultItem.ContainerId, Guid.Empty);
        //    Assert.True(resultItem.CreatedDate > DateTime.MinValue);
        //    Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

        //    //Clean
        //    dbContext.PageContent.RemoveRange(dbContext.PageContent);
        //}

        //[Fact]
        //public void GetByContainerFail()
        //{
        //    //Arrange
        //    var pageContentProvider = new PageContentProvider(container);
        //    var pageContents = TestDataProvider.GetPageContents();
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    foreach (var item in pageContents)
        //    {
        //        pageContentProvider.Create(item);
        //    }
        //    var pageContentId = Guid.NewGuid();

        //    //Act
        //    var result = pageContentProvider.GetByContainer(pageContentId);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Count == 0);
        //}

        [Fact]
        public void GetByPageLocaleSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageContentId = pageContents.First().PageId;
            var locale = pageContents.First().PageContentTranslation.First().CultureCode;

            //Act
            var result = pageContentProvider.Get(pageContentId, locale);
            var resultItem = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.NotNull(resultItem.PageContentTranslation);
            Assert.NotNull(resultItem.ContentType);
            Assert.NotNull(resultItem.ContentType.ContentDataType);
            Assert.NotNull(resultItem.ContentType.ContentTypeProperties);
            Assert.True(resultItem.ContentType.ContentTypeProperties.Count > 0);
            Assert.NotNull(resultItem.ContentPermissions);
            Assert.True(resultItem.ContentPermissions.Count > 0);
            Assert.True(!string.IsNullOrEmpty(resultItem.Properties));
            Assert.True(resultItem.SortOrder > 0);
            Assert.NotEqual(resultItem.ContainerId, Guid.Empty);
            Assert.True(resultItem.CreatedDate > DateTime.MinValue);
            Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void GetByPageLocaleFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageContentProvider.Get(pageId, "");

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetTranslationByContentIdSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageContentId = pageContents.First().Id;
            var locale = pageContents.First().PageContentTranslation.First().CultureCode;

            //Act
            var result = pageContentProvider.GetTranslations(pageContentId, locale);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.ContentData));
            Assert.True(!string.IsNullOrEmpty(result.CultureCode));
        }

        [Fact]
        public void GetTranslationByContentIdFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var pageContentId = Guid.NewGuid();
            var locale = "";

            //Act
            var result = pageContentProvider.GetTranslations(pageContentId, locale);

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void GetTranslationByIdSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var translationId = pageContents.First().PageContentTranslation.First().Id;

            //Act
            var result = pageContentProvider.GetTranslation(translationId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.ContentData));
            Assert.True(!string.IsNullOrEmpty(result.CultureCode));
        }

        [Fact]
        public void GetTranslationByIdFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents();
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }
            var translationId = Guid.NewGuid();

            //Act
            var result = pageContentProvider.GetTranslation(translationId);

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void CreatePageContentSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContents = TestDataProvider.GetPageContents();
            var pageContent = pageContents.First();


            //Act
            var result = pageContentProvider.Create(pageContent);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageContentTranslation);
            Assert.NotNull(result.ContentType);
            Assert.NotNull(result.ContentPermissions);
            Assert.True(result.ContentPermissions.Count > 0);
            Assert.True(!string.IsNullOrEmpty(result.Properties));
            Assert.True(result.SortOrder > 0);
            Assert.NotEqual(result.ContainerId, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void CreatePageContentFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContents = TestDataProvider.GetPageContents();
            PageContent pageContent = null;


            //Act
            var result = pageContentProvider.Create(pageContent);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void CreateTranslationSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContents = TestDataProvider.GetPageContents();
            var pageContentTranslation = pageContents.First().PageContentTranslation.First();


            //Act
            var result = pageContentProvider.CreateTranslation(pageContentTranslation);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.ContentData));
            Assert.True(!string.IsNullOrEmpty(result.CultureCode));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContentTranslation.RemoveRange(dbContext.PageContentTranslation);
        }

        [Fact]
        public void CreateTranslationFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContents = TestDataProvider.GetPageContents();
            PageContentTranslation pageContentTranslation = null;


            //Act
            var result = pageContentProvider.CreateTranslation(pageContentTranslation);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContentTranslation.RemoveRange(dbContext.PageContentTranslation);
        }

        [Fact]
        public void UpdatePageContentSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents(false);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                item.ContentPermissions = null;
                item.PageContentTranslation = null;
                pageContentProvider.Create(item);
            }

            var itemToUpdate = pageContents.First();
            itemToUpdate.IsDeleted = true;


            //Act
            var result = pageContentProvider.Update(itemToUpdate);
            
            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Properties));
            Assert.True(result.SortOrder > 0);
            Assert.NotEqual(result.ContainerId, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.True(result.IsDeleted == itemToUpdate.IsDeleted);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void UpdatePageContentFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContents = TestDataProvider.GetPageContents();
            PageContent pageContent = null;


            //Act
            var result = pageContentProvider.Update(pageContent);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void AddOrUpdateSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var pageContents = TestDataProvider.GetPageContents(false);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            foreach (var item in pageContents)
            {
                pageContentProvider.Create(item);
            }

            var itemToUpdate = pageContents.First();
            itemToUpdate.SortOrder = 10;
            var pageContentsToUpdate = TestDataProvider.GetPageContents().First();
            pageContents.Add(pageContentsToUpdate);

            //Act            
            var initialCount = dbContext.PageContent.Count();
            pageContentProvider.AddOrUpdate(pageContents);
            var afterCount = dbContext.PageContent.Count();
            var updatedItem = pageContentProvider.Get(itemToUpdate.Id);

            //Assert    
            Assert.True(afterCount > initialCount);
            Assert.True(updatedItem.SortOrder == itemToUpdate.SortOrder);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]

        public void AddOrUpdateFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);

            List<PageContent> itemToUpdate = null;

            //Act //Addert
            Assert.Throws(typeof(NullReferenceException), () => pageContentProvider.AddOrUpdate(itemToUpdate));
        }

        [Fact]
        public void AddContentPermissionsSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentPermissions = TestDataProvider.GetContentPermissions();

            //Act
            var result = pageContentProvider.AddContentPermissions(contentPermissions);            

            //Assert    
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }

        [Fact]
        public void AddContentPermissionsFail()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            List<ContentPermission> contentPermissions = null;

            //Act
            var result = pageContentProvider.AddContentPermissions(contentPermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }

        [Fact]
        public void UpdateContentPermissionAddSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContent = TestDataProvider.GetPageContents(false).First();
            pageContentProvider.Create(pageContent);
            var contentPermissions = TestDataProvider.GetContentPermissions();
            foreach(var cp in contentPermissions)
            {
                cp.PageContentId = pageContent.Id;
            }
            pageContent.ContentPermissions = contentPermissions;


            //Act
            pageContentProvider.UpdateContentPermission(pageContent);
            var result = pageContentProvider.Get(pageContent.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ContentPermissions.Count > 0);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }

        [Fact]
        public void UpdateContentPermissionRemoveSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContent = TestDataProvider.GetPageContents(false).First();
            pageContentProvider.Create(pageContent);
            var contentPermissions = TestDataProvider.GetContentPermissions();
            foreach (var cp in contentPermissions)
            {
                cp.PageContentId = pageContent.Id;
            }
            pageContent.ContentPermissions = contentPermissions;
            pageContentProvider.UpdateContentPermission(pageContent);
            pageContent.ContentPermissions.Remove(contentPermissions.First());


            //Act
            pageContentProvider.UpdateContentPermission(pageContent);
            var result = pageContentProvider.Get(pageContent.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ContentPermissions.Count == 1);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }

        [Fact]
        public void UpdateContentPermissionSuccess()
        {
            //Arrange
            var pageContentProvider = new PageContentProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageContent = TestDataProvider.GetPageContents(false).First();
            pageContentProvider.Create(pageContent);
            var contentPermissions = TestDataProvider.GetContentPermissions();
            foreach (var cp in contentPermissions)
            {   
                cp.PageContentId = pageContent.Id;
            }
            pageContent.InheritEditPermissions = pageContent.InheritViewPermissions = true;
            pageContent.ContentPermissions = contentPermissions;

            //Act
            pageContentProvider.UpdateContentPermission(pageContent);
            var result = pageContentProvider.Get(pageContent.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ContentPermissions.Count > 1);
            Assert.True(result.InheritViewPermissions);
            Assert.True(result.InheritEditPermissions);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }
    }
}
