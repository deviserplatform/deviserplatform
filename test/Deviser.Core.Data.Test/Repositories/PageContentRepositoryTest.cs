using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataRepositorys
{
    public class PageContentRepositoryTest : TestBase
    {
        [Fact]
        public void GetSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageContentId = pageContents.First().Id;


            //Act
            var result = pageContentRepository.Get(pageContentId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageContentTranslation);
            Assert.NotNull(result.ContentType);
            //Assert.NotNull(result.ContentType.ContentDataType);
            Assert.NotNull(result.ContentType.Properties);
            Assert.True(result.ContentType.Properties.Count > 0);
            Assert.NotNull(result.ContentPermissions);
            Assert.True(result.ContentPermissions.Count > 0);
            Assert.NotNull(result.Properties);
            Assert.True(result.Properties.Count>0);
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
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageContentId = Guid.NewGuid();

            //Act
            var result = pageContentRepository.Get(pageContentId);

            //Assert
            Assert.Null(result);
        }

        //[Fact]
        //public void GetByContainerSuccess()
        //{
        //    //Arrange
        //    var pageContentRepository = new PageContentRepository(container);
        //    var pageContents = TestDataRepository.GetPageContents();
        //    var dbContext = serviceRepository.GetRequiredService<DeviserDBContext>();
        //    foreach (var item in pageContents)
        //    {
        //        pageContentRepository.Create(item);
        //    }
        //    var pageContentId = pageContents.First().ContainerId;


        //    //Act
        //    var result = pageContentRepository.GetByContainer(pageContentId);
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
        //    var pageContentRepository = new PageContentRepository(container);
        //    var pageContents = TestDataRepository.GetPageContents();
        //    var dbContext = serviceRepository.GetRequiredService<DeviserDBContext>();
        //    foreach (var item in pageContents)
        //    {
        //        pageContentRepository.Create(item);
        //    }
        //    var pageContentId = Guid.NewGuid();

        //    //Act
        //    var result = pageContentRepository.GetByContainer(pageContentId);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Count == 0);
        //}

        [Fact]
        public void GetByPageLocaleSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageContentId = pageContents.First().PageId;
            var locale = pageContents.First().PageContentTranslation.First().CultureCode;

            //Act
            var result = pageContentRepository.Get(pageContentId, locale);
            var resultItem = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.NotNull(resultItem.PageContentTranslation);
            Assert.NotNull(resultItem.ContentType);
            //Assert.NotNull(resultItem.ContentType.ContentDataType);
            Assert.NotNull(resultItem.ContentType.Properties);
            Assert.True(resultItem.ContentType.Properties.Count > 0);
            Assert.NotNull(resultItem.ContentPermissions);
            Assert.True(resultItem.ContentPermissions.Count > 0);
            Assert.NotNull(resultItem.Properties);
            Assert.True(resultItem.Properties.Count > 0);
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
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageContentRepository.Get(pageId, "");

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetTranslationByContentIdSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageContentId = pageContents.First().Id;
            var locale = pageContents.First().PageContentTranslation.First().CultureCode;

            //Act
            var result = pageContentRepository.GetTranslations(pageContentId, locale);

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
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var pageContentId = Guid.NewGuid();
            var locale = "";

            //Act
            var result = pageContentRepository.GetTranslations(pageContentId, locale);

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void GetTranslationByIdSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var translationId = pageContents.First().PageContentTranslation.First().Id;

            //Act
            var result = pageContentRepository.GetTranslation(translationId);

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
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                pageContentRepository.Create(item);
            }
            var translationId = Guid.NewGuid();

            //Act
            var result = pageContentRepository.GetTranslation(translationId);

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void CreatePageContentSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContents = TestDataRepository.GetPageContents();
            var pageContent = pageContents.First();


            //Act
            var result = pageContentRepository.Create(pageContent);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageContentTranslation);
            Assert.NotNull(result.ContentType);
            Assert.NotNull(result.ContentPermissions);
            Assert.True(result.ContentPermissions.Count > 0);
            Assert.NotNull(result.Properties);
            Assert.True(result.Properties.Count > 0);
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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContents = TestDataRepository.GetPageContents();
            PageContent pageContent = null;


            //Act
            var result = pageContentRepository.Create(pageContent);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void CreateTranslationSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContents = TestDataRepository.GetPageContents();
            var pageContentTranslation = pageContents.First().PageContentTranslation.First();


            //Act
            var result = pageContentRepository.CreateTranslation(pageContentTranslation);

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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContents = TestDataRepository.GetPageContents();
            PageContentTranslation pageContentTranslation = null;


            //Act
            var result = pageContentRepository.CreateTranslation(pageContentTranslation);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContentTranslation.RemoveRange(dbContext.PageContentTranslation);
        }

        [Fact]
        public void UpdatePageContentSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents(false);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                item.ContentType.Properties = null;
                item.ContentPermissions = null;
                item.PageContentTranslation = null;
                pageContentRepository.Create(item);
            }
            var pageContent = pageContents.First();

            var itemToUpdate = pageContentRepository.Get(pageContent.Id);
            itemToUpdate.IsDeleted = true;


            //Act
            var result = pageContentRepository.Update(itemToUpdate);
            
            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.Properties);
            Assert.True(result.Properties.Count > 0);
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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContents = TestDataRepository.GetPageContents();
            PageContent pageContent = null;


            //Act
            var result = pageContentRepository.Update(pageContent);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void AddOrUpdateSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var pageContents = TestDataRepository.GetPageContents(false);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in pageContents)
            {
                item.ContentType.Properties = null;
                pageContentRepository.Create(item);
            }

            var itemToUpdate = pageContents.First();
            itemToUpdate.SortOrder = 10;
            var pageContentsToUpdate = TestDataRepository.GetPageContents().First();
            pageContents.Add(pageContentsToUpdate);

            //Act            
            var initialCount = dbContext.PageContent.Count();
            pageContentRepository.AddOrUpdate(pageContents);
            var afterCount = dbContext.PageContent.Count();
            var updatedItem = pageContentRepository.Get(itemToUpdate.Id);

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
            var pageContentRepository = new PageContentRepository(_container);

            List<PageContent> itemToUpdate = null;

            //Act //Assert
            Assert.Throws(typeof(NullReferenceException), () => pageContentRepository.AddOrUpdate(itemToUpdate));
        }

        [Fact]
        public void AddContentPermissionsSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentPermissions = TestDataRepository.GetContentPermissions();

            //Act
            var result = pageContentRepository.AddContentPermissions(contentPermissions);            

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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            List<ContentPermission> contentPermissions = null;

            //Act
            var result = pageContentRepository.AddContentPermissions(contentPermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.ContentPermission.RemoveRange(dbContext.ContentPermission);
        }

        [Fact]
        public void UpdateContentPermissionAddSuccess()
        {
            //Arrange
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContent = TestDataRepository.GetPageContents(false).First();
            pageContentRepository.Create(pageContent);
            var contentPermissions = TestDataRepository.GetContentPermissions();
            foreach(var cp in contentPermissions)
            {
                cp.PageContentId = pageContent.Id;
            }
            pageContent.ContentPermissions = contentPermissions;


            //Act
            pageContentRepository.UpdateContentPermission(pageContent);
            var result = pageContentRepository.Get(pageContent.Id);


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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContent = TestDataRepository.GetPageContents(false).First();
            pageContentRepository.Create(pageContent);
            var contentPermissions = TestDataRepository.GetContentPermissions();
            foreach (var cp in contentPermissions)
            {
                cp.PageContentId = pageContent.Id;
            }
            pageContent.ContentPermissions = contentPermissions;
            pageContentRepository.UpdateContentPermission(pageContent);
            pageContent.ContentPermissions.Remove(contentPermissions.First());


            //Act
            pageContentRepository.UpdateContentPermission(pageContent);
            var result = pageContentRepository.Get(pageContent.Id);


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
            var pageContentRepository = new PageContentRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageContent = TestDataRepository.GetPageContents(false).First();
            pageContentRepository.Create(pageContent);
            var contentPermissions = TestDataRepository.GetContentPermissions();
            foreach (var cp in contentPermissions)
            {   
                cp.PageContentId = pageContent.Id;
            }
            pageContent.InheritEditPermissions = pageContent.InheritViewPermissions = true;
            pageContent.ContentPermissions = contentPermissions;

            //Act
            pageContentRepository.UpdateContentPermission(pageContent);
            var result = pageContentRepository.Get(pageContent.Id);


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
