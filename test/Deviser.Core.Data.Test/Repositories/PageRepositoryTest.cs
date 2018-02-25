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
    public class PageRepositoryTest : TestBase
    {
        [Fact]
        public void GetPageTreeSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            Page parentPage = pages.First();
            pages.Remove(parentPage);
            pageRepository.CreatePage(parentPage);
            foreach (var page in pages)
            {
                page.ParentId = parentPage.Id;
                pageRepository.CreatePage(page);
            }

            //Act
            var root = pageRepository.GetPageTree();
            var result = root.ChildPage.First();
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
            Assert.NotNull(root.ChildPage);
            Assert.True(root.ChildPage.Count > 0);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTreeFail()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            dbContext.Page.RemoveRange(dbContext.Page);

            //Act
            var result = pageRepository.GetPageTree();

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            pageRepository.CreatePage(page);


            //Act
            var result = pageRepository.GetPage(page.Id);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            dbContext.Page.RemoveRange(dbContext.Page);


            //Act
            var result = pageRepository.GetPage(Guid.NewGuid());

            //Assert            
            Assert.Null(result);
        }

        [Fact]
        public void CreatePageSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();

            //Act
            var result = pageRepository.CreatePage(page);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            Page page = null;

            //Act
            var result = pageRepository.CreatePage(page);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePageSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pageToUpdate = pageRepository.CreatePage(page);
            var pageTranslation = pageToUpdate.PageTranslation.First();
            var pagePermissions = pageToUpdate.PagePermissions;
            pageTranslation.Name = "TestUpdate";

            //Act
            pageRepository.UpdatePage(pageToUpdate);
            var result = pageRepository.GetPage(pageToUpdate.Id);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pageToUpdate = pageRepository.CreatePage(page);
            var pagePermissions = pageToUpdate.PagePermissions;
            var initialCount = pageToUpdate.PagePermissions.Count;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageId = pageToUpdate.Id,
            });

            //Act
            pageRepository.UpdatePage(pageToUpdate);
            var result = pageRepository.GetPage(page.Id);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pageToUpdate = pageRepository.CreatePage(page);
            var pagePermissions = pageToUpdate.PagePermissions;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageId = pageToUpdate.Id
            });
            
            var initialCount = pageToUpdate.PagePermissions.Count;

            //Act
            pageToUpdate.PagePermissions.Remove(pageToUpdate.PagePermissions.First());
            pageRepository.UpdatePage(pageToUpdate);
            var result = pageRepository.GetPage(pageToUpdate.Id);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            Page page = null;

            //Act
            var result = pageRepository.UpdatePage(page);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void UpdatePageTreeSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var parentPage = TestDataRepository.GetPages().First();
            var childPages = TestDataRepository.GetPages();
            parentPage = pageRepository.CreatePage(parentPage);
            parentPage.ChildPage = new List<Page>();
            foreach (var page in childPages)
            {
                page.ParentId = parentPage.Id;
                var resultChildPage = pageRepository.CreatePage(page);
                parentPage.ChildPage.Add(resultChildPage);
            }

            //Act
            var pageToUpdate = parentPage.ChildPage.First();
            pageToUpdate.PageOrder = 5;
            var pageTranslation = pageToUpdate.PageTranslation.First();
            pageTranslation.Name = "TestUpdate";

            var result = pageRepository.UpdatePageTree(parentPage);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            Page parentPage = pages.First();
            pages.Remove(parentPage);
            pageRepository.CreatePage(parentPage);
            parentPage.ChildPage = new List<Page>();
            foreach (var page in pages)
            {
                page.ParentId = parentPage.Id;
                pageRepository.CreatePage(page);
                parentPage.ChildPage.Add(page);
            }

            //Act
            Page pageToUpdate = null;

            var result = pageRepository.UpdatePageTree(pageToUpdate);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationsSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pagePermissions = page.PagePermissions;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            pageRepository.CreatePage(page);
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageRepository.GetPageTranslations(pageTranslation.Locale);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pagePermissions = page.PagePermissions;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            pageRepository.CreatePage(page);
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageRepository.GetPageTranslations(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageTranslationSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pagePermissions = page.PagePermissions;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            pageRepository.CreatePage(page);
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageRepository.GetPageTranslation(pageTranslation.URL);

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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pages = TestDataRepository.GetPages();
            var page = pages.First();
            var pagePermissions = page.PagePermissions;
            pagePermissions.Add(new PagePermission
            {
                Id = Guid.NewGuid(),
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            pageRepository.CreatePage(page);
            var pageTranslation = page.PageTranslation.First();

            //Act
            var result = pageRepository.GetPageTranslations(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetPageModulesSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();
            pageRepository.CreatePageModule(pageModule);
            var pageId = pageModule.PageId;

            //Act
            var result = pageRepository.GetPageModules(pageId);
            var resultPageModule = result.First();
            var resultPermission = resultPageModule.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultPageModule);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            foreach (var pageModule in pageModules)
            {
                pageRepository.CreatePageModule(pageModule);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageRepository.GetPageModules(pageId);

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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();
            pageRepository.CreatePageModule(pageModule);
            var moduleId = pageModule.Id;

            //Act
            var result = pageRepository.GetPageModule(moduleId);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            foreach (var pageModule in pageModules)
            {
                pageRepository.CreatePageModule(pageModule);
            }
            var pageId = Guid.NewGuid();

            //Act
            var result = pageRepository.GetPageModule(pageId);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void CreatePageModuleSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageRepository.CreatePageModule(pageModule);
            var resultPermission = result.ModulePermissions.First();

            //Assert            
            Assert.NotNull(result);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageRepository.CreatePageModule(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModuleSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();
            var pageModuleToUpdate = pageRepository.CreatePageModule(pageModule);
            pageModuleToUpdate.SortOrder = 10;


            //Act
            var result = pageRepository.UpdatePageModule(pageModuleToUpdate);
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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            var pageModule = pageModules.First();

            //Act
            var result = pageRepository.CreatePageModule(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
        }

        [Fact]
        public void UpdatePageModulesSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModules = TestDataRepository.GetPageModules();
            foreach(var pageModule in pageModules)
            {
                pageModule.ModulePermissions = null;
                pageRepository.CreatePageModule(pageModule);
            }

            var pageModuleToUpdate = pageModules.First();
            pageModuleToUpdate.SortOrder = 10;
            var pageModulesToUpdate = new List<PageModule>
            {
                pageModuleToUpdate
            };

            //Act
            pageRepository.UpdatePageModules(pageModulesToUpdate);
            var result = pageRepository.GetPageModule(pageModuleToUpdate.Id);

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
            var pageRepository = new PageRepository(_container);

            List<PageModule> itemToUpdate = null;

            //Act //Addert
            Assert.Throws(typeof(NullReferenceException), () => pageRepository.UpdatePageModules(itemToUpdate));
        }

        [Fact]
        public void AddPagePermissionsSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pagePermissions = TestDataRepository.GetPagePermissions();

            //Act
            var result = pageRepository.AddPagePermissions(pagePermissions);

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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            List<PagePermission> pagePermissions = null;

            //Act
            var result = pageRepository.AddPagePermissions(pagePermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void AddModulePermissionsSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modulePermissions = TestDataRepository.GetModulePermissions();

            //Act
            var result = pageRepository.AddModulePermissions(modulePermissions);

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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            List<ModulePermission> pagePermissions = null;

            //Act
            var result = pageRepository.AddModulePermissions(pagePermissions);

            //Assert    
            Assert.Null(result);

            //Clean
            dbContext.PagePermission.RemoveRange(dbContext.PagePermission);
        }

        [Fact]
        public void UpdateModulePermissionAddSuccess()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModule = TestDataRepository.GetPageModules().First();
            pageRepository.CreatePageModule(pageModule);
            var modulePermissions = TestDataRepository.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
            pageModule.ModulePermissions = modulePermissions;


            //Act
            pageRepository.UpdateModulePermission(pageModule);
            var result = pageRepository.GetPageModule(pageModule.Id);


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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModule = TestDataRepository.GetPageModules().First();
            pageRepository.CreatePageModule(pageModule);
            var modulePermissions = TestDataRepository.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
            pageModule.ModulePermissions = modulePermissions;
            pageRepository.UpdateModulePermission(pageModule);

            pageModule.ModulePermissions.Remove(modulePermissions.First());


            //Act
            pageRepository.UpdateModulePermission(pageModule);
            var result = pageRepository.GetPageModule(pageModule.Id);


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
            var pageRepository = new PageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageModule = TestDataRepository.GetPageModules().First();
            pageRepository.CreatePageModule(pageModule);
            var modulePermissions = TestDataRepository.GetModulePermissions();
            foreach (var mp in modulePermissions)
            {
                mp.PageModuleId = pageModule.Id;
            }
                        
            pageModule.InheritEditPermissions = pageModule.InheritViewPermissions = true;
            pageModule.ModulePermissions = modulePermissions;

            //Act
            pageRepository.UpdateModulePermission(pageModule);
            var result = pageRepository.GetPageModule(pageModule.Id);


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
