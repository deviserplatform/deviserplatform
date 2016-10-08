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
    public class RoleProviderTest : TestBase
    {
        //[Fact]
        //public void GetRolesSuccess()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var roles = TestDataProvider.GetRoles();
        //    foreach (var item in roles)
        //    {
        //        roleProvider.CreateRole(item);
        //    }

        //    //Act
        //    var result = roleProvider.GetRoles();
        //    var resultItem = result.First();

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.True(result.Count > 0);
        //    Assert.NotNull(resultItem);
        //    Assert.NotEqual(resultItem.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(resultItem.Name));

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}


        [Fact]
        public void GetRolesFail()
        {
            //Arrange
            var roleProvider = new RoleProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Roles.RemoveRange(dbContext.Roles);

            //Act
            var result = roleProvider.GetRoles();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }
        
        //[Fact]
        //public void GetRoleSuccess()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var roles = TestDataProvider.GetRoles();
        //    foreach (var item in roles)
        //    {                
        //        roleProvider.CreateRole(item);
        //    }
        //    var roleId = roles.First().Id;

        //    //Act
        //    var result = roleProvider.GetRole(roleId);

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.NotNull(result);
        //    Assert.NotEqual(result.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(result.Name));

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}

        [Fact]
        public void GetRoleFail()
        {
            //Arrange
            var roleProvider = new RoleProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataProvider.GetRoles();
            foreach (var item in roles)
            {
                roleProvider.CreateRole(item);
            }
            var roleId = Guid.NewGuid();

            //Act
            var result = roleProvider.GetRole(roleId);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        //[Fact]
        //public void CreateSuccess()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var roles = TestDataProvider.GetRoles();
        //    var role = roles.First();

        //    //Act
        //    var result = roleProvider.CreateRole(role);

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.NotNull(result);
        //    Assert.NotEqual(result.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(result.Name));

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}

        //[Fact]
        //public void CreateFail()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var roles = TestDataProvider.GetRoles();
        //    Role role = null;

        //    //Act
        //    var result = roleProvider.CreateRole(role);

        //    //Assert            
        //    Assert.Null(result);

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}

        //[Fact]
        //public void UpdateSuccess()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();            
        //    var roles = TestDataProvider.GetRoles();
        //    foreach (var item in roles)
        //    {
        //        roleProvider.CreateRole(item);
        //    }
        //    var rolesToUpdate = roles.First();
        //    rolesToUpdate.Name = "UpdatedRole";

        //    //Act
        //    var result = roleProvider.UpdateRole(rolesToUpdate);

        //    //Assert            
        //    Assert.NotNull(result);
        //    Assert.NotNull(result);
        //    Assert.NotEqual(result.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(result.Name));
        //    Assert.True(result.Name == rolesToUpdate.Name);

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}

        //[Fact]
        //public void UpdateFail()
        //{
        //    //Arrange
        //    var roleProvider = new RoleProvider(container);
        //    var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
        //    var roles = TestDataProvider.GetRoles();
        //    foreach (var item in roles)
        //    {
        //        roleProvider.CreateRole(item);
        //    }
        //    Role role = null;

        //    //Act
        //    var result = roleProvider.UpdateRole(role);

        //    //Assert            
        //    Assert.Null(result);

        //    //Clean
        //    dbContext.Roles.RemoveRange(dbContext.Roles);
        //}
    }
}
