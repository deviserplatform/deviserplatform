using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Xunit;

namespace Deviser.Core.Data.Test.DataProviders
{
    public class UserProviderTest : TestBase
    {
        [Fact]
        public void GetUsersSuccess()
        {
            //Arrange
            var userProvider = new UserProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var users = TestDataProvider.GetUsers();
            foreach (var item in users)
            {
                dbContext.Users.Add(Mapper.Map<Entities.User>(item));
            }
            dbContext.SaveChanges();

            //Act
            var result = userProvider.GetUsers();
            var resultItem = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(resultItem.Email));

            //Clean
            dbContext.Users.RemoveRange(dbContext.Users);
        }


        [Fact]
        public void GetUsersFail()
        {
            //Arrange
            var userProvider = new UserProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Users.RemoveRange(dbContext.Users);

            //Act
            var result = userProvider.GetUsers();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.Users.RemoveRange(dbContext.Users);
        }

        [Fact]
        public void GetUserSuccess()
        {
            //Arrange
            var userProvider = new UserProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var users = TestDataProvider.GetUsers();
            foreach (var item in users)
            {
                dbContext.Users.Add(Mapper.Map<Entities.User>(item));
            }
            dbContext.SaveChanges();
            var userId = users.First().Id;

            //Act
            var result = userProvider.GetUser(userId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Email));

            //Clean
            dbContext.Users.RemoveRange(dbContext.Users);
        }


        [Fact]
        public void GetUserFail()
        {
            // Arrange
            var userProvider = new UserProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var users = TestDataProvider.GetUsers();
            foreach (var item in users)
            {
                dbContext.Users.Add(Mapper.Map<Entities.User>(item));
            }
            dbContext.SaveChanges();
            var userId = Guid.Empty;

            //Act
            var result = userProvider.GetUser(userId);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Users.RemoveRange(dbContext.Users);
        }
    }
}
