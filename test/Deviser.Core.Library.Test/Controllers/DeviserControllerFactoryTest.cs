using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.TestCommon;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Deviser.Core.Library.Test.Controllers
{
    public class DeviserControllerFactoryTest : TestBase
    {
        [Fact]
        public void GetPageModuleResultsSuccess()
        {
            var actionContextMock = new Mock<ActionContext>();
            var scopeServiceMock = new Mock<IScopeService>();
            var pageManager = new PageManager(container);

            var pageId = SetupPageAndModules();
            var currentPage = pageManager.GetPage(pageId);

            scopeServiceMock.Setup(s => s.PageContext.CurrentPage).Returns(currentPage);
            var deviserControllerFactory = new DeviserControllerFactory(container, scopeServiceMock.Object);
                
            var result = deviserControllerFactory.GetPageModuleResults(actionContextMock.Object).Result;
            var contentResults = result.First().Value;
            var resultItem = contentResults.First();

            Assert.NotNull(result);
            Assert.NotNull(result.Count>0);
            Assert.NotNull(contentResults);
            Assert.NotNull(contentResults.Count>0);
            Assert.NotNull(resultItem);
            Assert.True(!string.IsNullOrEmpty(resultItem.Result));
        }

        private Guid SetupPageAndModules()
        {
            //Arrange
            var pageProvider = new PageProvider(container);
            var moduleProvider = new ModuleProvider(container);
            
            var pages = TestDataProvider.GetPages();
            var page = pages.First();

            //Create a page
            pageProvider.CreatePage(page);

            //Create modules
            var modules = TestDataProvider.GetModules();
            foreach (var module in modules)
            {
                moduleProvider.Create(module);

                //Create page modules
                var pageModule = new PageModule()
                {
                    Id = Guid.NewGuid(),
                    PageId = page.Id,
                    ContainerId = Guid.NewGuid(),
                    InheritViewPermissions = true,
                    InheritEditPermissions = true,
                    ModuleId = module.Id,
                    ModuleAction = module.ModuleAction.First(),
                    ModulePermissions = new List<ModulePermission>()
                    {
                        new ModulePermission
                        {
                            Id = Guid.NewGuid(),
                            PageModuleId = Guid.NewGuid(),
                            PermissionId = Guid.NewGuid(),
                            RoleId = Guid.NewGuid(),
                        }
                    },
                    IsDeleted = false
                };
                pageProvider.CreatePageModule(pageModule);
            }

            return page.Id;
        }
    }
}
