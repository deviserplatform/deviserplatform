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
            Assert.True(afterCount > initialCount);

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
            Assert.True(afterCount < initialCount);

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

        [Fact]
        public void UpdatePageTreeSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            Page parentPage = pages.First();
            pages.Remove(parentPage);
            pageProvider.CreatePage(parentPage);
            parentPage.ChildPage = new List<Page>();
            foreach (var page in pages)
            {
                page.ParentId = parentPage.Id;
                pageProvider.CreatePage(page);
                parentPage.ChildPage.Add(page);
            }

            //Act
            var pageToUpdate = parentPage.ChildPage.First();
            pageToUpdate.PageOrder = 5;
            var pageTranslation = pageToUpdate.PageTranslation.First();
            var pagePermissions = pageToUpdate.PagePermissions;
            pageTranslation.Name = "TestUpdate";

            var result = pageProvider.UpdatePage(parentPage);
            var resultPage = result.ChildPage.First(p => p.Id == pageToUpdate.Id);
            var resultPageTranslation = resultPage.PageTranslation.First();

            //Assert            
            Assert.NotNull(resultPage);
            Assert.NotEqual(resultPage.Id, Guid.Empty);
            Assert.True(resultPage.CreatedDate > DateTime.MinValue);
            Assert.True(resultPage.LastModifiedDate > DateTime.MinValue);
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.URL));
            Assert.NotNull(result.ChildPage);
            Assert.True(result.ChildPage.Count > 0);
            Assert.True(resultPage.PageOrder == pageToUpdate.PageOrder);
            Assert.True(pageTranslation.Name == resultPageTranslation.Name);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePageTreeFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pages = TestDataProvider.GetPages();
            Page parentPage = pages.First();
            pages.Remove(parentPage);
            pageProvider.CreatePage(parentPage);
            parentPage.ChildPage = new List<Page>();
            foreach (var page in pages)
            {
                page.ParentId = parentPage.Id;
                pageProvider.CreatePage(page);
                parentPage.ChildPage.Add(page);
            }

            //Act
            Page pageToUpdate = null;

            var result = pageProvider.UpdatePage(pageToUpdate);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationsSuccess()
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
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageProvider.GetPageTranslations(pageTranslation.Locale);
            var resultPageTranslation = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Name));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Description));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Locale));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.Title));
            Assert.True(!string.IsNullOrEmpty(resultPageTranslation.URL));

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationsFail()
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
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageProvider.GetPageTranslations(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationSuccess()
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
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageProvider.GetPageTranslation(pageTranslation.URL);

            //Assert            
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Description));
            Assert.True(!string.IsNullOrEmpty(result.Locale));
            Assert.True(!string.IsNullOrEmpty(result.Title));
            Assert.True(!string.IsNullOrEmpty(result.URL));

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationFail()
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
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageProvider.GetPageTranslations(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageModulesSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();
            pageProvider.CreatePageModule(pageModule);
            var pageId = pageModule.PageId;

            //Act
            var result = pageProvider.GetPageModules(pageId);
            var resultPageModule = result.First();
            var resultPermission = resultPageModule.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultPageModule);
            Assert.NotNull(resultPageModule.Module);
            Assert.NotNull(resultPageModule.ModuleAction);
            Assert.NotNull(resultPageModule.ModulePermissions);
            Assert.True(resultPageModule.ModulePermissions.Count > 0);
            Assert.NotNull(resultPermission);
            Assert.True(resultPermission.Id != Guid.Empty);
            Assert.True(resultPermission.PermissionId != Guid.Empty);
            Assert.True(resultPermission.RoleId != Guid.Empty);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void GetPageModulesFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            foreach (var pageModule in pageModules)
            {
                pageProvider.CreatePageModule(pageModule);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageProvider.GetPageModules(pageId);

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void GetPageModuleSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();
            pageProvider.CreatePageModule(pageModule);
            var moduleId = pageModule.Id;

            //Act
            var result = pageProvider.GetPageModule(moduleId);
            var resultPermission = result.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.ModulePermissions);
            Assert.True(result.ModulePermissions.Count > 0);
            Assert.NotNull(resultPermission);
            Assert.True(resultPermission.Id != Guid.Empty);
            Assert.True(resultPermission.PermissionId != Guid.Empty);
            Assert.True(resultPermission.RoleId != Guid.Empty);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void GetPageModuleFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            foreach (var pageModule in pageModules)
            {
                pageProvider.CreatePageModule(pageModule);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageProvider.GetPageModule(pageId);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void CreatePageModuleSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageProvider.CreatePageModule(pageModule);
            var resultPermission = result.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.Module);
            Assert.NotNull(result.ModuleAction);
            Assert.NotNull(result.ModulePermissions);
            Assert.True(result.ModulePermissions.Count > 0);
            Assert.NotNull(resultPermission);
            Assert.True(resultPermission.Id != Guid.Empty);
            Assert.True(resultPermission.PermissionId != Guid.Empty);
            Assert.True(resultPermission.RoleId != Guid.Empty);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void CreatePageModuleFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageProvider.CreatePageModule(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModuleSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();
            var pageModuleToUpdate = pageProvider.CreatePageModule(pageModule);
            pageModuleToUpdate.SortOrder = 10;


            //Act
            var result = pageProvider.UpdatePageModule(pageModule);
            var resultPermission = result.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.SortOrder == pageModuleToUpdate.SortOrder);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModuleFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageProvider.CreatePageModule(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModulesSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModules = TestDataProvider.GetPageModules();
            foreach(var pageModule in pageModules)
            {
                pageModule.ModulePermissions = null;
                pageProvider.CreatePageModule(pageModule);
            }

            var pageModuleToUpdate = pageModules.First();
            pageModuleToUpdate.SortOrder = 10;

            //Act
            pageProvider.UpdatePageModules(pageModules);
            var result = pageProvider.GetPageModule(pageModuleToUpdate.Id);

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.SortOrder == pageModuleToUpdate.SortOrder);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModulesFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);

            List<PageModule> itemToUpdate = null;

            //Act //Addert
            Assert.Throws(typeof(NullReferenceException), () => pageProvider.UpdatePageModules(itemToUpdate));
        }

        [Fact]
        public void AddPagePermissionsSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pagePermissions = TestDataProvider.GetPagePermissions();

            //Act
            var result = pageProvider.AddPagePermissions(pagePermissions);

            //Assert    
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void AddPagePermissionsFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            List<PagePermission> pagePermissions = null;

            //Act
            var result = pageProvider.AddPagePermissions(pagePermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void AddModulePermissionsSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var modulePermissions = TestDataProvider.GetModulePermissions();

            //Act
            var result = pageProvider.AddModulePermissions(modulePermissions);

            //Assert    
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void AddModulePermissionsFail()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            List<ModulePermission> pagePermissions = null;

            //Act
            var result = pageProvider.AddModulePermissions(pagePermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void UpdateModulePermissionAddSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModule = TestDataProvider.GetPageModules().First();
            pageProvider.CreatePageModule(pageModule);
            var modulePermissions = TestDataProvider.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
            pageModule.ModulePermissions = modulePermissions;


            //Act
            pageProvider.UpdateModulePermission(pageModule);
            var result = pageProvider.GetPageModule(pageModule.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ModulePermissions.Count > 0);

            //Clean
            dbContext.ModulePermission.RemoveRange(dbContext.ModulePermission);
        }

        [Fact]
        public void UpdateModulePermissionRemoveSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModule = TestDataProvider.GetPageModules().First();
            pageProvider.CreatePageModule(pageModule);
            var modulePermissions = TestDataProvider.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
            pageModule.ModulePermissions = modulePermissions;
            pageProvider.UpdateModulePermission(pageModule);

            pageModule.ModulePermissions.Remove(modulePermissions.First());


            //Act
            pageProvider.UpdateModulePermission(pageModule);
            var result = pageProvider.GetPageModule(pageModule.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ModulePermissions.Count == 1);

            //Clean
            dbContext.ModulePermission.RemoveRange(dbContext.ModulePermission);
        }

        [Fact]
        public void UpdateModulePermissionSuccess()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var pageModule = TestDataProvider.GetPageModules().First();
            pageProvider.CreatePageModule(pageModule);
            var modulePermissions = TestDataProvider.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
                        
            pageModule.InheritEditPermissions = pageModule.InheritViewPermissions = true;
            pageModule.ModulePermissions = modulePermissions;

            //Act
            pageProvider.UpdateModulePermission(pageModule);
            var result = pageProvider.GetPageModule(pageModule.Id);


            //Assert    
            Assert.NotNull(result);
            Assert.True(result.ModulePermissions.Count > 1);
            Assert.True(result.InheritViewPermissions);
            Assert.True(result.InheritEditPermissions);

            //Clean
            dbContext.ModulePermission.RemoveRange(dbContext.ModulePermission);
        }

    }
}
