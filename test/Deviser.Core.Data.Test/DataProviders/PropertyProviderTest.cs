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
    public class PropertyProviderTest : TestBase
    {
        [Fact]
        public void CreatePagePropertySuccess()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            var property = properties.First();


            //Act
            var result = propertyProvider.CreateProperty(property);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.Value));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        [Fact]
        public void CreatePagePropertyFail()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            Property property = nulls;


            //Act
            var result = propertyProvider.CreateProperty(property);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.Value));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }

        public void GetPropertiesSuccess()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            foreach (var item in properties)
            {
                propertyProvider.CreateProperty(item);
            }

            //Act
            var result = propertyProvider.GetProperties();
            var resultItem = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(resultItem.Name));
            Assert.True(!string.IsNullOrEmpty(resultItem.Label));
            Assert.True(!string.IsNullOrEmpty(resultItem.Value));
            Assert.True(resultItem.CreatedDate > DateTime.MinValue);
            Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.PageContent.RemoveRange(dbContext.PageContent);
        }
    }
}
