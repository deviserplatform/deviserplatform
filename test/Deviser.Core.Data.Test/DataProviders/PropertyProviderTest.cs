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
        public void CreatePropertySuccess()
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
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void CreatePropertyFail()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            Property property = null;


            //Act
            var result = propertyProvider.CreateProperty(property);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
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
            Assert.True(resultItem.CreatedDate > DateTime.MinValue);
            Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void GetPropertiesFail()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            dbContext.Property.RemoveRange(dbContext.Property);

            //Act
            var result = propertyProvider.GetProperties();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }


        [Fact]
        public void GetPropertySuccess()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            var property = properties.First();
            propertyProvider.CreateProperty(property);


            //Act
            var result = propertyProvider.GetProperty(property.Id);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void GetPropertyFail()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();

            //Act
            var result = propertyProvider.GetProperty(Guid.NewGuid());

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void UpdatePropertySuccess()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();
            foreach (var item in properties)
            {
                propertyProvider.CreateProperty(item);
            }
            var propertyToUpdate = properties.First();
            propertyToUpdate.Value = "Updated Value";

            //Act
            var result = propertyProvider.UpdateProperty(propertyToUpdate);
            
            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(propertyToUpdate.Value == result.Value);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void UpdatePropertyFail()
        {
            //Arrange
            var propertyProvider = new PropertyProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var properties = TestDataProvider.GetProperties();

            //Act
            var result = propertyProvider.UpdateProperty(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }
    }
}
