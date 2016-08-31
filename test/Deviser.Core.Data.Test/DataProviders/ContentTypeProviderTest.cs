using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Data.Entities;
using Deviser.WI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Deviser.Core.Data.DataProviders
{
    public class ContentTypeProviderTest
    {
        private readonly ILifetimeScope container;
        private readonly IServiceProvider serviceProvider;

        public ContentTypeProviderTest()
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            var services = new ServiceCollection();
            services.AddOptions();
            services
                .AddDbContext<DeviserDBContext>(b => b.UseInMemoryDatabase().UseInternalServiceProvider(efServiceProvider));

            //services.AddIdentity<User, Role>()
            //        .AddEntityFrameworkStores<DeviserDBContext>();

            services.AddLogging();
            services.AddOptions();

            // IHttpContextAccessor is required for SignInManager, and UserManager
            //var context = new DefaultHttpContext();
            //context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature() { Handler = new TestAuthHandler() });
            //services.AddSingleton<IHttpContextAccessor>(
            //    new HttpContextAccessor()
            //    {
            //        HttpContext = context,
            //    });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            container = containerBuilder.Build();
            serviceProvider = new AutofacServiceProvider(container);
        }

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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentTypes = GetContentTypes();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentTypes = GetContentTypes();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentTypes = GetContentTypes();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentDataTypes = GetContentDataTypes();
            dbContext.ContentDataType.AddRange(contentDataTypes);
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentTypes = GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }
            var contentTypeToUpdate = contentTypes.First();
            contentTypeToUpdate.ContentTypeProperties = new List<ContentTypeProperty>();
            var cssProp = new Property()
            {
                Id = Guid.NewGuid(),
                Name = "cssclass",
                Label = "Css Class"
            };
            dbContext.Property.Add(cssProp);

            contentTypeToUpdate.ContentTypeProperties.Add(new ContentTypeProperty
            {
                ConentTypeId = contentTypeToUpdate.Id,
                PropertyId = cssProp.Id,
            });

            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeProvider.UpdateContentType(contentTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == contentTypeToUpdate.Label);
            Assert.True(result.ContentTypeProperties.Count > 0);

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
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            var contentTypes = GetContentTypes();
            foreach (var ct in contentTypes)
            {
                contentTypeProvider.CreateContentType(ct);
            }
            var contentTypeToUpdate = contentTypes.First();
            contentTypeToUpdate.ContentTypeProperties = new List<ContentTypeProperty>();
            var cssProp = new Property()
            {
                Id = Guid.NewGuid(),
                Name = "cssclass",
                Label = "Css Class"
            };
            var heightProp = new Property()
            {
                Id = Guid.NewGuid(),
                Name = "height",
                Label = "Height"
            };

            dbContext.Property.Add(cssProp);
            dbContext.ContentTypeProperty.Add(new ContentTypeProperty
            {
                ConentTypeId = contentTypeToUpdate.Id,
                PropertyId = cssProp.Id,
            });
            dbContext.ContentTypeProperty.Add(new ContentTypeProperty
            {
                ConentTypeId = contentTypeToUpdate.Id,
                PropertyId = heightProp.Id,
            });

            //Act
            contentTypeToUpdate.Label = "New Label";
            var result = contentTypeProvider.UpdateContentType(contentTypeToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Label == contentTypeToUpdate.Label);
            Assert.True(result.ContentTypeProperties.Count == 0);

            //Clean
            dbContext.Property.RemoveRange(dbContext.Property);
            dbContext.ContentTypeProperty.RemoveRange(dbContext.ContentTypeProperty);
            dbContext.ContentType.RemoveRange(dbContext.ContentType);
        }

        private List<ContentType> GetContentTypes()
        {   
            var contentTypes = new List<ContentType>();
            var stringType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "string",
                Label = "string"
            };
            var objectType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "object",
                Label = "object"
            };
            var arrayType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "array",
                Label = "array"
            };
            
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Text",
                Label = "Text",
                ContentDataTypeId = stringType.Id
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Image",
                Label = "Image",
                ContentDataTypeId = objectType.Id
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "RichText",
                Label = "Rich text",
                ContentDataTypeId = stringType.Id
            });

            return contentTypes;
        }

        private List<ContentDataType> GetContentDataTypes()
        {
            var contentDataTypes = new List<ContentDataType>();
            var stringType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "string",
                Label = "string"
            };
            var objectType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "object",
                Label = "object"
            };
            var arrayType = new ContentDataType
            {
                Id = Guid.NewGuid(),
                Name = "array",
                Label = "array"
            };

            contentDataTypes.Add(stringType);
            contentDataTypes.Add(objectType);
            contentDataTypes.Add(arrayType);
            return contentDataTypes;
        }

        //private class TestAuthHandler : IAuthenticationHandler
        //{
        //    public void Authenticate(AuthenticateContext context)
        //    {
        //        context.NotAuthenticated();
        //    }

        //    public Task AuthenticateAsync(AuthenticateContext context)
        //    {
        //        context.NotAuthenticated();
        //        return Task.FromResult(0);
        //    }

        //    public Task ChallengeAsync(ChallengeContext context)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void GetDescriptions(DescribeSchemesContext context)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task SignInAsync(SignInContext context)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task SignOutAsync(SignOutContext context)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
