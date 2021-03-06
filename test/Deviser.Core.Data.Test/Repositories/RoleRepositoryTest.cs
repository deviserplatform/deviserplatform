﻿using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataRepositorys
{
    public class RoleRepositoryTest : TestBase
    {
        [Fact]
        public void GetRolesSuccess()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            foreach (var item in roles)
            {
                roleRepository.CreateRole(item);
            }

            //Act
            var result = roleRepository.GetRoles();
            var resultItem = result.First();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(resultItem.Name));

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }


        [Fact]
        public void GetRolesFail()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Roles.RemoveRange(dbContext.Roles);

            //Act
            var result = roleRepository.GetRoles();

            //Assert            
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void GetRoleSuccess()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            foreach (var item in roles)
            {
                roleRepository.CreateRole(item);
            }
            var roleId = roles.First().Id;

            //Act
            var result = roleRepository.GetRole(roleId);

            //Assert            
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void GetRoleFail()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            foreach (var item in roles)
            {
                roleRepository.CreateRole(item);
            }
            var roleId = Guid.NewGuid();

            //Act
            var result = roleRepository.GetRole(roleId);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void CreateSuccess()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            var role = roles.First();

            //Act
            var result = roleRepository.CreateRole(role);

            //Assert            
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void CreateFail()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            Role role = null;

            //Act
            var result = roleRepository.CreateRole(role);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void UpdateSuccess()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            foreach (var item in roles)
            {
                roleRepository.CreateRole(item);
            }
            var rolesToUpdate = roles.First();
            rolesToUpdate.Name = "UpdatedRole";

            //Act
            var result = roleRepository.UpdateRole(rolesToUpdate);

            //Assert            
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(result.Name == rolesToUpdate.Name);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }

        [Fact]
        public void UpdateFail()
        {
            //Arrange
            var roleRepository = new RoleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var roles = TestDataRepository.GetRoles();
            foreach (var item in roles)
            {
                roleRepository.CreateRole(item);
            }
            Role role = null;

            //Act
            var result = roleRepository.UpdateRole(role);

            //Assert            
            Assert.Null(result);

            //Clean
            dbContext.Roles.RemoveRange(dbContext.Roles);
        }
    }
}
