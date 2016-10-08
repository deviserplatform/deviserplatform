using Autofac;
using Autofac.Extensions.DependencyInjection;
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

namespace Deviser.Core.Data.DataProviders
{
    public class ContentTypeProviderTest : TestBase
    {
        [Fact]
        public void CreateContentTypeSuccess()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var contentType = new ContentType
            {
                Name = "TypeName",
                Label = "Type Name",
            };

            //Act
            var result = contentTypeProvider.CreateContentType(contentType);

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
            ContentTypeProvider contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            ContentType contentType = null;

            //Act
            var result = contentTypeProvider.CreateContentType(contentType);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContentTypeSuccess()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataProvider.GetContentTypes();
            foreach(var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }
            var id = contentTypes.First().Id;

            //Act
            var result = contentTypeProvider.GetContentType(id);

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
        public void GetContentTypeSuccess(string typeName)
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataProvider.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }

            //Act
            var result = contentTypeProvider.GetContentType(typeName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        [Fact]
        public void GetContentTypeFail()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var id = Guid.Empty;

            //Act
            var result = contentTypeProvider.GetContentType(id);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("SkyType")]
        public void GetContentTypeFail(string typeName)
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);

            //Act
            var result = contentTypeProvider.GetContentType(typeName);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetContentTypesSuccess()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataProvider.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }

            //Act
            var result = contentTypeProvider.GetContentTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        [Fact]
        public void GetContentDataTypesSuccess()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentDataTypes = TestDataProvider.GetContentDataTypes();
            var dbContentDataType = Mapper.Map<Deviser.Core.Data.Entities.ContentDataType>(contentDataTypes);
            dbContext.ContentDataType.AddRange(dbContentDataType);
            dbContext.SaveChanges();

            //Act
            var result = contentTypeProvider.GetContentDataTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.ContentDataType.RemoveRange(dbContext.ContentDataType);
        }

        [Fact]
        public void GetContentTypesFail()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.RemoveRange(dbContext.ContentType);

            //Act
            var result = contentTypeProvider.GetContentTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetContentDataTypesFail()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.RemoveRange(dbContext.ContentDataType);

            //Act
            var result = contentTypeProvider.GetContentDataTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void UpdateContentTypePropAddSuccess()
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataProvider.GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }

            var contentTypeToUpdate = contentTypes.First();
            contentTypeToUpdate.Properties = new List<Property>();
            var properties = TestDataProvider.GetProperties();
            var cssProp = properties[0];
            contentTypeToUpdate.Properties.Add(cssProp);
            
            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeProvider.UpdateContentType(contentTypeToUpdate);

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
            var contentTypeProvider = new ContentTypeProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var contentTypes = TestDataProvider.GetContentTypes();
            var properties = TestDataProvider.GetProperties();
            var cssProp = properties[0];
            var heightProp = properties[1];

            foreach (var ct in contentTypes)
            {
                ct.Properties = new List<Property>();
                ct.Properties.Add(cssProp);
                ct.Properties.Add(heightProp);
                contentTypeProvider.CreateContentType(ct);
            }
            var contentTypeToUpdate = contentTypes.First();
            contentTypeToUpdate.Properties = new List<Property>();
            
            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeProvider.UpdateContentType(contentTypeToUpdate);

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