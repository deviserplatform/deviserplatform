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
    public class OptionListRepositoryTest : TestBase
    {
        [Fact]
        public void CreateOptionListSuccess()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var optionList = optionLists.First();

            //Act
            var result = optionListRepository.CreateOptionList(optionList);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(result.List!=null);
            Assert.True(result.List.Count>0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }

        [Fact]
        public void CreateOptionListFail()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            OptionList optionList = null;

            //Act
            var result = optionListRepository.CreateOptionList(optionList);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetOptionListsSuccess()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListRepository.CreateOptionList(item);
            }

            //Act
            var result = optionListRepository.GetOptionLists();
            var resultItem = result.First();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.False(resultItem.IsActive);
            Assert.True(!string.IsNullOrEmpty(resultItem.Name));
            Assert.True(!string.IsNullOrEmpty(resultItem.Label));
            Assert.True(resultItem.List != null);
            Assert.True(resultItem.List.Count > 0);
            Assert.True(resultItem.CreatedDate > DateTime.MinValue);
            Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.OptionList.RemoveRange(dbContext.OptionList);
        }

        [Fact]
        public void GetOptionListsFail()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.OptionList.RemoveRange(dbContext.OptionList);

            //Act
            var result = optionListRepository.GetOptionLists();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetOptionListSuccess()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListRepository.CreateOptionList(item);
            }
            var listId = optionLists.First().Id;


            //Act
            var result = optionListRepository.GetOptionList(listId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(result.List != null);
            Assert.True(result.List.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.OptionList.RemoveRange(dbContext.OptionList);
        }

        [Fact]
        public void GetOptionListFail()
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListRepository.CreateOptionList(item);
            }
            var optionListId = Guid.NewGuid();

            //Act
            var result = optionListRepository.GetOptionList(optionListId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("YesNo")]
        [InlineData("YesNoNa")]
        public void GetOptionListByNameSuccess(string typeName)
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListRepository.CreateOptionList(item);
            }

            //Act
            var result = optionListRepository.GetOptionList(typeName);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(result.List != null);
            Assert.True(result.List.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.OptionList.RemoveRange(dbContext.OptionList);
        }

        [Theory]
        [InlineData("UnknownType")]
        public void GetOptionListByNameFail(string typeName)
        {
            //Arrange
            var optionListRepository = new OptionListRepository(_container);
            var optionLists = TestDataRepository.GetOptionLists();
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListRepository.CreateOptionList(item);
            }

            //Act
            var result = optionListRepository.GetOptionList(typeName);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.OptionList.RemoveRange(dbContext.OptionList);
        }
    }
}
