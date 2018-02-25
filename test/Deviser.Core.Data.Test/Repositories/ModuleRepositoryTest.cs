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
    public class ModuleRepositoryTest : TestBase
    {
        [Fact]
        public void CreateModuleSuccess()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            var module = modules.First();

            //Act
            var result = moduleRepository.Create(module);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.Description));
            Assert.True(!string.IsNullOrEmpty(result.Version));
            Assert.NotNull(result.ModuleAction);
            Assert.True(result.ModuleAction.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Fact]
        public void CreateModuleFail()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            Module module = null;

            //Act
            var result = moduleRepository.Create(module);

            //Assert
            Assert.Null(result);
        }

        //[Fact]
        //public void CreateModuleActionSuccess()
        //{
        //    //Arrange
        //    var moduleRepository = new ModuleRepository(container);
        //    var moduleActions = TestDataRepository.GetModuleActions();
        //    var moduleAction = moduleActions.First();

        //    //Act
        //    var result = moduleRepository.Create(moduleAction);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.NotEqual(result.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(result.ActionName));
        //    Assert.True(!string.IsNullOrEmpty(result.ControllerName));
        //    Assert.True(!string.IsNullOrEmpty(result.ControllerNamespace));
        //    Assert.True(!string.IsNullOrEmpty(result.DisplayName));
        //    Assert.True(!string.IsNullOrEmpty(result.IconClass));
        //    Assert.NotNull(result.ModuleActionType);
        //    Assert.True(result.ModuleActionTypeId!= Guid.Empty);
        //}

        //[Fact]
        //public void CreateModuleActionFail()
        //{
        //    //Arrange
        //    var moduleRepository = new ModuleRepository(container);
        //    ModuleAction moduleAction = null;

        //    //Act
        //    var result = moduleRepository.Create(moduleAction);

        //    //Assert
        //    Assert.Null(result);
        //}

        [Fact]
        public void GetModulesSuccess()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }

            //Act
            var result = moduleRepository.Get();
            var resultItem = result.First();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);            
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.False(resultItem.IsActive);
            Assert.True(!string.IsNullOrEmpty(resultItem.Name));
            Assert.True(!string.IsNullOrEmpty(resultItem.Label));
            Assert.True(!string.IsNullOrEmpty(resultItem.Description));
            Assert.True(!string.IsNullOrEmpty(resultItem.Version));
            Assert.NotNull(resultItem.ModuleAction);
            Assert.True(resultItem.ModuleAction.Count > 0);
            Assert.True(resultItem.CreatedDate > DateTime.MinValue);
            Assert.True(resultItem.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Fact]
        public void GetModulesFail()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Module.RemoveRange(dbContext.Module);

            //Act
            var result = moduleRepository.Get();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        //public void GetModuleActionsSuccess()
        //{
        //    //Arrange
        //    var moduleRepository = new ModuleRepository(container);
        //    var dbContext = serviceRepository.GetRequiredService<DeviserDBContext>();
        //    var moduleActions = TestDataRepository.GetModuleActions();
        //    foreach (var item in moduleActions)
        //    {
        //        moduleRepository.Create(item);
        //    }

        //    //Act
        //    var result = moduleRepository.GetModuleActions();
        //    var resultItem = result.First();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Count > 0);
        //    Assert.NotNull(resultItem);
        //    Assert.NotEqual(resultItem.Id, Guid.Empty);
        //    Assert.True(!string.IsNullOrEmpty(resultItem.ActionName));
        //    Assert.True(!string.IsNullOrEmpty(resultItem.ControllerName));
        //    Assert.True(!string.IsNullOrEmpty(resultItem.ControllerNamespace));
        //    Assert.True(!string.IsNullOrEmpty(resultItem.DisplayName));
        //    Assert.True(!string.IsNullOrEmpty(resultItem.IconClass));
        //    Assert.NotNull(resultItem.ModuleActionType);
        //    Assert.True(resultItem.ModuleActionTypeId != Guid.Empty);

        //    //Clean
        //    dbContext.ModuleAction.RemoveRange(dbContext.ModuleAction);
        //}

        [Fact]
        public void GetModuleActionsFail()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.ModuleAction.RemoveRange(dbContext.ModuleAction);

            //Act
            var result = moduleRepository.GetModuleActions();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetEditModuleActionsSuccess()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                item.ModuleAction.First().ModuleActionType.ControlType = "edit";
                moduleRepository.Create(item);
            }
            var moduleId = modules.First().Id;

            //Act
            var result = moduleRepository.GetEditModuleActions(moduleId);
            var resultItem = result.First();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.NotNull(resultItem);
            Assert.NotEqual(resultItem.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(resultItem.ActionName));
            Assert.True(!string.IsNullOrEmpty(resultItem.ControllerName));
            Assert.True(!string.IsNullOrEmpty(resultItem.ControllerNamespace));
            Assert.True(!string.IsNullOrEmpty(resultItem.DisplayName));
            Assert.True(!string.IsNullOrEmpty(resultItem.IconClass));
            Assert.True(resultItem.ModuleActionTypeId != Guid.Empty);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Fact]
        public void GetEditModuleActionsFail()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }
            var moduleId = Guid.NewGuid();

            //Act
            var result = moduleRepository.GetEditModuleActions(moduleId);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Fact]
        public void GetModuleSuccess()
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }
            var moduleId = modules.First().Id;

            //Act
            var result = moduleRepository.Get(moduleId);
            

            //Assert            
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.False(result.IsActive);
            Assert.True(!string.IsNullOrEmpty(result.Name));
            Assert.True(!string.IsNullOrEmpty(result.Label));
            Assert.True(!string.IsNullOrEmpty(result.Description));
            Assert.True(!string.IsNullOrEmpty(result.Version));
            Assert.NotNull(result.ModuleAction);
            Assert.True(result.ModuleAction.Count > 0);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Fact]
        public void GetModuleFail()
        {//Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }
            var moduleId = Guid.NewGuid();

            //Act
            var result = moduleRepository.Get(moduleId);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("Security")]
        [InlineData("Language")]
        public void GetModleByNameSuccess(string moduleName)
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }

            //Act
            var result = moduleRepository.Get(moduleName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        [Theory]        
        [InlineData("UnknownType")]
        public void GetContentTypeFail(string typeName)
        {
            //Arrange
            var moduleRepository = new ModuleRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var modules = TestDataRepository.GetModules();
            foreach (var item in modules)
            {
                moduleRepository.Create(item);
            }

            //Act
            var result = moduleRepository.Get(typeName);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Module.RemoveRange(dbContext.Module);
        }

        //TODO: Update test
    }

    
}
