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
            Assert.NotNull(result.PageTranslation.Count>0);
            Assert.NotNull(result.PagePermissions);
            Assert.NotNull(result.PagePermissions.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }
        
        //[Fact]
        //public void CreatePageFail()
        //{
        //    //Arrange
        //    var pageProvider = new PageProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var pages = TestDataProvider.GetPages();
        //    Page parentPage = null;
        //    foreach(var page in pages)
        //    {
        //        pageProvider.CreatePage(page)
        //    }
        //    var page = pages.First();

        //    //Act
        //    var result = pageProvider.CreatePage(page);
        //    var pageTranslation = result.PageTranslation.First();

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.NotEqual(result.Id, Guid.Empty);
        //    Assert.NotNull(result.PageTranslation);
        //    Assert.NotNull(result.PageTranslation.Count > 0);
        //    Assert.NotNull(result.PagePermissions);
        //    Assert.NotNull(result.PagePermissions.Count > 0);
        //    Assert.True(result.CreatedDate > DateTime.MinValue);
        //    Assert.True(result.LastModifiedDate > DateTime.MinValue);

        //    //Clean
        //    dbContext.PageContent.RemoveRange(dbContext.PageContent);
        //}
    }
}
