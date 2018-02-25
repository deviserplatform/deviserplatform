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
    public class LayoutTypeRepositoryTest : TestBase
    {
        [Fact]
        public void CreateLayoutTypeSuccess()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            var layoutType = layoutTypes.First();

            //Act
            var result = layoutTypeRepository.CreateLayoutType(layoutType);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.LayoutTypeIds));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }

        [Fact]
        public void CreateLayoutFail()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            LayoutType layoutType = null;

            //Act
            var result = layoutTypeRepository.CreateLayoutType(layoutType);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLayoutTypesSuccess()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeRepository.GetLayoutTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }

        [Fact]
        public void GetLayoutsFail()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);

            //Act
            var result = layoutTypeRepository.GetLayoutTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLayoutSuccess()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }
            var id = layoutTypes.First().Id;

            //Act
            var result = layoutTypeRepository.GetLayoutType(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.LayoutTypeIds));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }

        [Fact]
        public void GetLayoutFail()
        {//Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }
            var id = Guid.Empty;

            //Act
            var result = layoutTypeRepository.GetLayoutType(id);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }

        [Theory]
        [InlineData("container")]
        [InlineData("row")]
        [InlineData("column")]
        public void GetLayoutTypeSuccess(string typeName)
        {
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeRepository.GetLayoutType(typeName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.LayoutTypeIds));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }

        [Theory]
        [InlineData("SkyType")]
        public void GetLayoutTypeFail(string typeName)
        {
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeRepository.GetLayoutType(typeName);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateLayoutTypePropAddSuccess()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeRepository.CreateLayoutType(item);
            }
            var layoutTypeToUpdate = layoutTypes.First();

            layoutTypeToUpdate.Properties = new List<Property>();
            var properties = TestDataRepository.GetProperties();
            var cssProp = properties[0];

            layoutTypeToUpdate.Properties.Add(cssProp);

            //Act
            layoutTypeToUpdate.Label = "New Label";
            var result = layoutTypeRepository.UpdateLayoutType(layoutTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == layoutTypeToUpdate.Label);
            Assert.True(result.Properties.Count > 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
            dbContext.LayoutTypeProperty.RemoveRange(dbContext.LayoutTypeProperty);
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }

        [Fact]
        public void UpdateLayoutTypePropRemoveSuccess()
        {
            //Arrange
            var layoutTypeRepository = new LayoutTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataRepository.GetLayoutTypes();
            var properties = TestDataRepository.GetProperties();
            var cssProp = properties[0];
            var heightProp = properties[1];

            foreach (var item in layoutTypes)
            {
                item.Properties = new List<Property>();
                item.Properties.Add(cssProp);
                item.Properties.Add(heightProp);
                layoutTypeRepository.CreateLayoutType(item);
            }
            var layoutTypeToUpdate = layoutTypes.First();
            layoutTypeToUpdate.Properties = new List<Property>();
            

            //Act
            layoutTypeToUpdate.Label = "New Label";
            var result = layoutTypeRepository.UpdateLayoutType(layoutTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == layoutTypeToUpdate.Label);
            Assert.True(result.Properties.Count == 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
            dbContext.LayoutTypeProperty.RemoveRange(dbContext.LayoutTypeProperty);
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);
        }
    }
}
