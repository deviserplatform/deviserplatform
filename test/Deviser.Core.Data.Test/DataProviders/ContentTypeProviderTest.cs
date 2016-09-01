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

            SetupData();
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
            var id = GetContentTypes().First().Id;

            //Act
            var result = contentTypeProvider.GetContentType(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }

        [Fact]
        [InlineData("Text")]
        [InlineData("Image")]
        [InlineData("RichText")]
        public void GetContentTypeSuccess(string typeName)
        {
            //Arrange
            var contentTypeProvider = new ContentTypeProvider(container);

            //Act
            var result = contentTypeProvider.GetContentType(typeName);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
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

        [Fact]
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

            //Act
            var result = contentTypeProvider.GetContentTypes();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
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
            Assert.True(result.Count > 0);
        }

        private void SetupData()
        {
            var dbContext = serviceProvider.GetRequiredService<DeviserDBContext>();
            List<ContentType> contentTypes = GetContentTypes();
            var contentTypeProvider = new ContentTypeProvider(container);
            foreach(var contentType in contentTypes)
            {
                contentTypeProvider.CreateContentType(contentType);
            }
        }

        private List<ContentType> GetContentTypes()
        {
            List<ContentType> contentTypes = new List<ContentType>();
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Text",
                Label = "Text",
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Image",
                Label = "Image",
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "RichText",
                Label = "Rich text",
            });
            return contentTypes;
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
