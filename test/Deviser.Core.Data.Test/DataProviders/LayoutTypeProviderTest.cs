using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataProviders
{
    public class LayoutTypeProviderTest : TestBase
    {
        [Fact]
        public void CreateLayoutTypeSuccess()
        {
            //Arrange
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            var layoutType = layoutTypes.First();

            //Act
            var result = layoutTypeProvider.CreateLayoutType(layoutType);

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            LayoutType layoutType = null;

            //Act
            var result = layoutTypeProvider.CreateLayoutType(layoutType);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLayoutTypesSuccess()
        {
            //Arrange
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeProvider.GetLayoutTypes();

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.LayoutType.RemoveRange(dbContext.LayoutType);

            //Act
            var result = layoutTypeProvider.GetLayoutTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLayoutSuccess()
        {
            //Arrange
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }
            var id = layoutTypes.First().Id;

            //Act
            var result = layoutTypeProvider.GetLayoutType(id);

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }
            var id = Guid.Empty;

            //Act
            var result = layoutTypeProvider.GetLayoutType(id);

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeProvider.GetLayoutType(typeName);

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }

            //Act
            var result = layoutTypeProvider.GetLayoutType(typeName);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateLayoutTypePropAddSuccess()
        {
            //Arrange
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            foreach (var item in layoutTypes)
            {
                layoutTypeProvider.CreateLayoutType(item);
            }
            var layoutTypeToUpdate = layoutTypes.First();

            layoutTypeToUpdate.Properties = new List<Property>();
            var properties = TestDataProvider.GetProperties();
            var cssProp = properties[0];

            layoutTypeToUpdate.Properties.Add(cssProp);

            //Act
            layoutTypeToUpdate.Label = "New Label";
            var result = layoutTypeProvider.UpdateLayoutType(layoutTypeToUpdate);

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
            var layoutTypeProvider = new LayoutTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layoutTypes = TestDataProvider.GetLayoutTypes();
            var properties = TestDataProvider.GetProperties();
            var cssProp = properties[0];
            var heightProp = properties[1];

            foreach (var item in layoutTypes)
            {
                item.Properties = new List<Property>();
                item.Properties.Add(cssProp);
                item.Properties.Add(heightProp);
                layoutTypeProvider.CreateLayoutType(item);
            }
            var layoutTypeToUpdate = layoutTypes.First();
            layoutTypeToUpdate.Properties = new List<Property>();
            

            //Act
            layoutTypeToUpdate.Label = "New Label";
            var result = layoutTypeProvider.UpdateLayoutType(layoutTypeToUpdate);

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
