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
    public class OptionListProviderTest : TestBase
    {
        [Fact]
        public void CreateOptionListSuccess()
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var optionList = optionLists.First();

            //Act
            var result = optionListProvider.CreateOptionList(optionList);

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
            var optionListProvider = new OptionListProvider(container);
            PropertyOptionList optionList = null;

            //Act
            var result = optionListProvider.CreateOptionList(optionList);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetOptionListsSuccess()
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListProvider.CreateOptionList(item);
            }

            //Act
            var result = optionListProvider.GetOptionLists();
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
            dbContext.PropertyOptionList.RemoveRange(dbContext.PropertyOptionList);
        }

        [Fact]
        public void GetOptionListsFail()
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.PropertyOptionList.RemoveRange(dbContext.PropertyOptionList);

            //Act
            var result = optionListProvider.GetOptionLists();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetOptionListSuccess()
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListProvider.CreateOptionList(item);
            }
            var listId = optionLists.First().Id;


            //Act
            var result = optionListProvider.GetOptionList(listId);

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
            dbContext.PropertyOptionList.RemoveRange(dbContext.PropertyOptionList);
        }

        [Fact]
        public void GetOptionListFail()
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListProvider.CreateOptionList(item);
            }
            var optionListId = Guid.NewGuid();

            //Act
            var result = optionListProvider.GetOptionList(optionListId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("YesNo")]
        [InlineData("YesNoNa")]
        public void GetOptionListByNameSuccess(string typeName)
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListProvider.CreateOptionList(item);
            }

            //Act
            var result = optionListProvider.GetOptionList(typeName);

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
            dbContext.PropertyOptionList.RemoveRange(dbContext.PropertyOptionList);
        }

        [Theory]
        [InlineData("UnknownType")]
        public void GetOptionListByNameFail(string typeName)
        {
            //Arrange
            var optionListProvider = new OptionListProvider(container);
            var optionLists = TestDataProvider.GetPropertyOptionLists();
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            foreach (var item in optionLists)
            {
                optionListProvider.CreateOptionList(item);
            }

            //Act
            var result = optionListProvider.GetOptionList(typeName);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.PropertyOptionList.RemoveRange(dbContext.PropertyOptionList);
        }
    }
}
