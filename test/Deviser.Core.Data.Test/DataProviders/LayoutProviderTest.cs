using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataProviders
{
    public class LayoutProviderTest : TestBase
    {
        [Fact]
        public void CreateLayoutSuccess()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var layouts = TestDataProvider.GetLayouts();
            var layout = layouts.First();

            //Act
            var result = layoutProvider.CreateLayout(layout);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsDeleted);
            Assert.True(!string.IsNullOrEmpty(result.Config));
            Assert.True(!string.IsNullOrEmpty(result.Name));
        }

        [Fact]
        public void CreateLayoutFail()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);            
            Layout layout = null;

            //Act
            var result = layoutProvider.CreateLayout(layout);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLayoutsSuccess()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataProvider.GetLayouts();
            foreach (var item in layouts)
            {
                layoutProvider.CreateLayout(item);
            }

            //Act
            var result = layoutProvider.GetLayouts();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

        [Fact]
        public void GetLayoutsFail()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Layout.RemoveRange(dbContext.Layout);

            //Act
            var result = layoutProvider.GetLayouts();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLayoutSuccess()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataProvider.GetLayouts();
            foreach (var item in layouts)
            {
                layoutProvider.CreateLayout(item);
            }
            var id = layouts.First().Id;

            //Act
            var result = layoutProvider.GetLayout(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Config));

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

        [Fact]
        public void GetLayoutFail()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataProvider.GetLayouts();
            foreach (var item in layouts)
            {
                layoutProvider.CreateLayout(item);
            }
            var id = Guid.Empty;

            //Act
            var result = layoutProvider.GetLayout(id);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

        [Fact]
        public void UpdateLayoutSuccess()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataProvider.GetLayouts();
            foreach (var item in layouts)
            {
                layoutProvider.CreateLayout(item);
            }

            var layoutToUpdate = layoutProvider.GetLayouts().First();

            //Act
            layoutToUpdate.IsDeleted = true;
            layoutToUpdate.Name = "NewName";
            layoutToUpdate.Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\"]";

            var result = layoutProvider.UpdateLayout(layoutToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == layoutToUpdate.Name);
            Assert.True(result.Config == layoutToUpdate.Config);
            Assert.True(result.IsDeleted == layoutToUpdate.IsDeleted);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

        [Fact]
        public void UpdateLayoutFail()
        {
            //Arrange
            var layoutProvider = new LayoutProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataProvider.GetLayouts();
            foreach (var item in layouts)
            {
                layoutProvider.CreateLayout(item);
            }

            Layout layoutToUpdate = null;

            //Act
            var result = layoutProvider.UpdateLayout(layoutToUpdate);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

    }
}
