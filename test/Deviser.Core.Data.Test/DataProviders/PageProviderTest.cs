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
    public class PageProviderTest : TestBase
    {
        [Fact]
        public void GetPageTreeSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            Page parentPage = pages.First();
            pages.Remove(parentPage);
            pageProvider.CreatePage(parentPage);
            foreach (var page in pages)
            {
                page.ParentId = parentPage.Id;
                pageProvider.CreatePage(page);
            }

            //Act
            var result = pageProvider.GetPageTree();
            var pageTranslation = result.PageTranslation.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.URL));
            Assert.NotNull(result.ChildPage);
            Assert.True(result.ChildPage.Count > 0);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTreeFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            dbContext.Page.RemoveRange(dbContext.Page);

            //Act
            var result = pageProvider.GetPageTree();

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();
            pageProvider.CreatePage(page);


            //Act
            var result = pageProvider.GetPage(page.Id);
            var resultPageTranslation = result.PageTranslation.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.NotNull(resultPageTranslation);
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.URL));

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();
            dbContext.Page.RemoveRange(dbContext.Page);


            //Act
            var result = pageProvider.GetPage(Guid.NewGuid());

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void CreatePageSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();

            //Act
            var result = pageProvider.CreatePage(page);
            var pageTranslation = result.PageTranslation.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.NotNull(pageTranslation);
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(pageTranslation.URL));

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void CreatePageFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            Page page = null;

            //Act
            var result = pageProvider.CreatePage(page);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePageSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();
            pageProvider.CreatePage(page);
            var pageTranslation = page.PageTranslation.First();
            var pagePermissions = page.PagePermissions;
            pageTranslation.Name = "TestUpdate";

            //Act
            pageProvider.UpdatePage(page);
            var result = pageProvider.GetPage(page.Id); 
            var resultPageTranslation = result.PageTranslation.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.NotNull(resultPageTranslation);
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.URL));
            Assert.Equal(pageTranslation.Name, resultPageTranslation.Name);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePagePermissionAddSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();
            pageProvider.CreatePage(page);
            var pagePermissions = page.PagePermissions;
            var initialCount = page.PagePermissions.Count;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageId = page.Id,
            });

            //Act
            pageProvider.UpdatePage(page);
            var result = pageProvider.GetPage(page.Id);
            var afterCount = result.PagePermissions.Count;

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.True(afterCount> initialCount);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePagePermissionRemoveSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            var page = pages.First();            
            var pagePermissions = page.PagePermissions;            
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            pageProvider.CreatePage(page);
            var initialCount = page.PagePermissions.Count;

            //Act
            page.PagePermissions.Remove(page.PagePermissions.First());
            pageProvider.UpdatePage(page);
            var result = pageProvider.GetPage(page.Id);
            var afterCount = result.PagePermissions.Count;

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotNull(result.PageTranslation);
            Assert.True(result.PageTranslation.Count > 0);
            Assert.NotNull(result.PagePermissions);
            Assert.True(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
            Assert.True(afterCount< initialCount);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePageFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            Page page = null;

            //Act
            var result = pageProvider.UpdatePage(page);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }
    }
}
