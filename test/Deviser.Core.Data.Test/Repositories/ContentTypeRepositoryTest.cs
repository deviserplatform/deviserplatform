using Deviser.TestCommon;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Xunit;
using ContentType = Deviser.Core.Common.DomainTypes.ContentType;

namespace Deviser.Core.Data.Repositories
{
    public class ContentTypeRepositoryTest : TestBase
    {
        [Fact]
        public void CreateContentTypeSuccess()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var contentType = new ContentType
            {
                Name = "TypeName",
                Label = "Type Name",
            };

            //Act
            var result = contentTypeRepository.CreateContentType(contentType);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }

        [Fact]
        public void CreateContentTypeFail()
        {
            //Arrange
            ContentTypeRepository contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            ContentType contentType = null;

            //Act
            var result = contentTypeRepository.CreateContentType(contentType);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContentTypeByIdSuccess()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataRepository.GetContentTypes();
            foreach(var ct in contentTypes)
            {
                contentTypeRepository.CreateContentType(ct);
            }
            var id = contentTypeRepository.GetContentTypes().First().Id;

            //Act
            var result = contentTypeRepository.GetContentType(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        [Theory]
        [InlineData("Text")]
        [InlineData("Image")]
        [InlineData("RichText")]
        public void GetContentTypeByNameSuccess(string typeName)
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataRepository.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeRepository.CreateContentType(ct);
            }

            //Act
            var result = contentTypeRepository.GetContentType(typeName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        [Fact]
        public void GetContentTypeByIdFail()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var id = Guid.Empty;

            //Act
            var result = contentTypeRepository.GetContentType(id);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("SkyType")]
        public void GetContentTypeByNameFail(string typeName)
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);

            //Act
            var result = contentTypeRepository.GetContentType(typeName);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContentTypesSuccess()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataRepository.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeRepository.CreateContentType(ct);
            }

            //Act
            var result = contentTypeRepository.GetContentTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        //[Fact]
        //public void GetContentDataTypesSuccess()
        //{
        //    //Arrange
        //    var contentTypeRepository = new ContentTypeRepository(container);
        //    var dbContext = serviceRepository.GetRequiredService<DeviserDbContext>();
        //    var contentDataTypes = TestDataRepository.GetContentDataTypes();
        //    var dbContentDataType = Mapper.Map<List<Entities.ContentDataType>>(contentDataTypes);
        //    dbContext.ContentDataType.AddRange(dbContentDataType);
        //    dbContext.SaveChanges();

        //    //Act
        //    var result = contentTypeRepository.GetContentDataTypes();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Count > 0);

        //    //Clean
        //    dbContext.ContentDataType.RemoveRange(dbContext.ContentDataType);
        //}

        [Fact]
        public void GetContentTypesFail()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.RemoveRange(dbContext.ContentType);

            //Act
            var result = contentTypeRepository.GetContentTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        //[Fact]
        //public void GetContentDataTypesFail()
        //{
        //    //Arrange
        //    var contentTypeRepository = new ContentTypeRepository(container);
        //    var dbContext = serviceRepository.GetRequiredService<DeviserDbContext>();
        //    dbContext.RemoveRange(dbContext.ContentDataType);

        //    //Act
        //    var result = contentTypeRepository.GetContentDataTypes();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.Count == 0);
        //}

        [Fact]
        public void UpdateContentTypePropAddSuccess()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataRepository.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeRepository.CreateContentType(ct);
            }

            var contentTypeToUpdate = contentTypeRepository.GetContentTypes().First();
            contentTypeToUpdate.Properties = new List<Property>();
            var properties = TestDataRepository.GetProperties();
            var cssProp = properties[0];
            contentTypeToUpdate.Properties.Add(cssProp);
            
            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeRepository.UpdateContentType(contentTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == contentTypeToUpdate.Label);
            Assert.True(result.Properties.Count > 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
            dbContext.ContentTypeProperty.RemoveRange(dbContext.ContentTypeProperty);
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        [Fact]
        public void UpdateContentTypePropRemoveSuccess()
        {
            //Arrange
            var contentTypeRepository = new ContentTypeRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataRepository.GetContentTypes();
            var properties = TestDataRepository.GetProperties();
            var cssProp = properties[0];
            var heightProp = properties[1];

            foreach (var ct in contentTypes)
            {
                ct.Properties = new List<Property>();
                ct.Properties.Add(cssProp);
                ct.Properties.Add(heightProp);
                contentTypeRepository.CreateContentType(ct);
            }
            var contentTypeToUpdate = contentTypeRepository.GetContentTypes().First();
            contentTypeToUpdate.Properties = new List<Property>();
            
            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeRepository.UpdateContentType(contentTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == contentTypeToUpdate.Label);
            Assert.True(result.Properties.Count == 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
            dbContext.ContentTypeProperty.RemoveRange(dbContext.ContentTypeProperty);
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }
    }
}