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
    public class PropertyRepositoryTest : TestBase
    {
        [Fact]
        public void CreatePropertySuccess()
        {
            //Arrange
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();
            var property = properties.First();


            //Act
            var result = propertyRepository.CreateProperty(property);

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
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();
            Property property = null;


            //Act
            var result = propertyRepository.CreateProperty(property);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void GetPropertiesSuccess()
        {
            //Arrange
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();
            foreach (var item in properties)
            {
                propertyRepository.CreateProperty(item);
            }

            //Act
            var result = propertyRepository.GetProperties();
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
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Property.RemoveRange(dbContext.Property);

            //Act
            var result = propertyRepository.GetProperties();

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
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();
            var property = properties.First();
            propertyRepository.CreateProperty(property);


            //Act
            var result = propertyRepository.GetProperty(property.Id);

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
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();

            //Act
            var result = propertyRepository.GetProperty(Guid.NewGuid());

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }

        [Fact]
        public void UpdatePropertySuccess()
        {
            //Arrange
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();
            foreach (var item in properties)
            {
                propertyRepository.CreateProperty(item);
            }
            var propertyToUpdate = propertyRepository.GetProperties().First();
            propertyToUpdate.Value = "Updated Value";

            //Act
            var result = propertyRepository.UpdateProperty(propertyToUpdate);
            
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
            var propertyRepository = new PropertyRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var properties = TestDataRepository.GetProperties();

            //Act
            var result = propertyRepository.UpdateProperty(null);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
        }
    }
}
