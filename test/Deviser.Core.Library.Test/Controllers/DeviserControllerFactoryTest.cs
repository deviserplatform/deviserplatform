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
using Deviser.Core.Data;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace Deviser.Core.Library.Test.Controllers
{
    public class DeviserControllerFactoryTest : TestBase
    {
        [Fact]
        public void GetPageModuleResultsSuccess()
        {
            //Arrange
            var actionContextMock = new Mock<ActionContext>();
            var scopeServiceMock = new Mock<IScopeService>();
            var pageManager = new PageManager(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageId = SetupPageAndModules();
            var currentPage = pageManager.GetPage(pageId);
            scopeServiceMock.Setup(s => s.PageContext.CurrentPage).Returns(currentPage);
            actionContextMock.Setup(ac => ac.RouteData.Routers).Returns(new List<IRouter>());
            var deviserControllerFactory = new DeviserControllerFactory(_container, scopeServiceMock.Object);

            //Act
            var result = deviserControllerFactory.GetPageModuleResults(actionContextMock.Object).Result;
            var contentResults = result.First().Value;
            var resultItem = contentResults.First();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Count > 0);
            Assert.NotNull(contentResults);
            Assert.NotNull(contentResults.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotNull(resultItem.HtmlResult);
            Assert.True(!GetString(resultItem.HtmlResult).Contains("Module load exception"));

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.Module.RemoveRange(dbContext.Module);
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        [Fact]
        public void GetPageModuleResultsFail()
        {
            //Arrange
            var actionContextMock = new Mock<ActionContext>();
            var scopeServiceMock = new Mock<IScopeService>();
            var pageManager = new PageManager(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageId = SetupPageAndModules();
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.SaveChanges();
            var currentPage = pageManager.GetPage(pageId);
            scopeServiceMock.Setup(s => s.PageContext.CurrentPage).Returns(currentPage);
            actionContextMock.Setup(ac => ac.RouteData.Routers).Returns(new List<IRouter>());
            var deviserControllerFactory = new DeviserControllerFactory(_container, scopeServiceMock.Object);

            //Act
            var result = deviserControllerFactory.GetPageModuleResults(actionContextMock.Object).Result;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Count == 0);

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.Module.RemoveRange(dbContext.Module);
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetModuleEditResultSuccess()
        {
            //Arrange
            //var actionContextMock = new Mock<ActionContext>();
            var scopeServiceMock = new Mock<IScopeService>();
            var pageManager = new PageManager(_container);
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageId = SetupPageAndModules();
            var currentPage = pageManager.GetPage(pageId);
            scopeServiceMock.Setup(s => s.PageContext.CurrentPage).Returns(currentPage);


            var httpContext = CreateHttpContext("GET");

            var router = new Mock<IRouter>(MockBehavior.Strict).Object;
            var actionContext = new ActionContext();
            actionContext.HttpContext = httpContext;
            actionContext.RouteData = new RouteData();
            actionContext.RouteData.Routers.Add(router);

            //actionContextMock.Setup(ac => ac.RouteData).Returns(new RouteData());
            //actionContextMock.Setup(ac => ac.RouteData.Routers).Returns(new List<IRouter>());
            var deviserControllerFactory = new DeviserControllerFactory(_container, scopeServiceMock.Object);
            var modules = moduleRepository.Get();
            var editModule = modules.First(m => m.ModuleAction.Any(ma => ma.ControllerName == "Edit"));
            var editModuleAction =
                editModule.ModuleAction.First(
                    ma => ma.ModuleActionTypeId == Guid.Parse("192278B6-7BF2-40C2-A776-B9CA5FB04FBB"));
            var pageModule = currentPage.PageModule.First(pm => pm.ModuleId == editModule.Id);

            //Act
            var result = deviserControllerFactory.GetModuleEditResultAsString(actionContext, pageModule, editModuleAction.Id).Result;
            
            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrEmpty(result));
            Assert.True(!result.Contains("Module load exception"));

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.Module.RemoveRange(dbContext.Module);
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        [Fact]
        public void GetModuleEditResultFail()
        {
            //Arrange
            var actionContextMock = new Mock<ActionContext>();
            var scopeServiceMock = new Mock<IScopeService>();
            var pageManager = new PageManager(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var pageId = SetupPageAndModules();
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.SaveChanges();
            var currentPage = pageManager.GetPage(pageId);
            scopeServiceMock.Setup(s => s.PageContext.CurrentPage).Returns(currentPage);
            actionContextMock.Setup(ac => ac.RouteData.Routers).Returns(new List<IRouter>());
            var deviserControllerFactory = new DeviserControllerFactory(_container, scopeServiceMock.Object);

            //Act
            var result = deviserControllerFactory.GetModuleEditResultAsString(actionContextMock.Object, new PageModule(), Guid.NewGuid()).Result;

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrEmpty(result));
            Assert.True(result.Contains("Module load exception"));

            //Clean
            dbContext.PageModule.RemoveRange(dbContext.PageModule);
            dbContext.Module.RemoveRange(dbContext.Module);
            dbContext.Page.RemoveRange(dbContext.Page);
        }

        private Guid SetupPageAndModules()
        {
            //Arrange
            var pageRepository = new PageRepository(_container);
            var moduleRepository = new ModuleRepository(_container);

            var pages = TestDataRepository.GetPages();
            var page = pages.First();

            //Create a page
            pageRepository.CreatePage(page);

            //Create modules
            var modules = TestDataRepository.GetModules();
            foreach (var module in modules)
            {
                moduleRepository.Create(module);

                if (module != null && module.ModuleAction != null)
                {
                    foreach (var moduleAction in module.ModuleAction)
                    {
                        //Create page modules
                        var pageModule = new PageModule()
                        {
                            Id = Guid.NewGuid(),
                            PageId = page.Id,
                            ContainerId = Guid.NewGuid(),
                            InheritViewPermissions = true,
                            InheritEditPermissions = true,
                            ModuleId = module.Id,
                            ModuleAction = moduleAction,
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
                        pageRepository.CreatePageModule(pageModule);
                    }
                }
            }

            return page.Id;
        }
    }
}
