using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataRepositorys
{
    public class LayoutRepositoryTest : TestBase
    {
        [Fact]
        public void CreateLayoutSuccess()
        {
            //Arrange
            var layoutRepository = new LayoutRepository(_container);
            var layouts = TestDataRepository.GetLayouts();
            var layout = layouts.First();

            //Act
            var result = layoutRepository.CreateLayout(layout);

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
            var layoutRepository = new LayoutRepository(_container);            
            Layout layout = null;

            //Act
            var result = layoutRepository.CreateLayout(layout);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLayoutsSuccess()
        {
            //Arrange
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataRepository.GetLayouts();
            foreach (var item in layouts)
            {
                layoutRepository.CreateLayout(item);
            }

            //Act
            var result = layoutRepository.GetLayouts();

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
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Layout.RemoveRange(dbContext.Layout);

            //Act
            var result = layoutRepository.GetLayouts();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLayoutSuccess()
        {
            //Arrange
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataRepository.GetLayouts();
            foreach (var item in layouts)
            {
                layoutRepository.CreateLayout(item);
            }
            var id = layouts.First().Id;

            //Act
            var result = layoutRepository.GetLayout(id);

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
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataRepository.GetLayouts();
            foreach (var item in layouts)
            {
                layoutRepository.CreateLayout(item);
            }
            var id = Guid.Empty;

            //Act
            var result = layoutRepository.GetLayout(id);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

        [Fact]
        public void UpdateLayoutSuccess()
        {
            //Arrange
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataRepository.GetLayouts();
            foreach (var item in layouts)
            {
                layoutRepository.CreateLayout(item);
            }

            var layoutToUpdate = layoutRepository.GetLayouts().First();

            //Act
            layoutToUpdate.IsDeleted = true;
            layoutToUpdate.Name = "NewName";
            layoutToUpdate.Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\"]";

            var result = layoutRepository.UpdateLayout(layoutToUpdate);

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
            var layoutRepository = new LayoutRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var layouts = TestDataRepository.GetLayouts();
            foreach (var item in layouts)
            {
                layoutRepository.CreateLayout(item);
            }

            Layout layoutToUpdate = null;

            //Act
            var result = layoutRepository.UpdateLayout(layoutToUpdate);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Layout.RemoveRange(dbContext.Layout);
        }

    }
}
