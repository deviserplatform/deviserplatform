using System;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Installation;
using Xunit;

namespace Deviser.Core.Data.Test.Installation
{
    public class DataSeederTest //: TestBase
    {
        [Fact]
        public void CreateLanguageSuccess()
        {

            var options = new DbContextOptionsBuilder<DeviserDbContext>()
                .UseInMemoryDatabase(databaseName: "DeviserWI")
                .Options;
            var dbContext = new DeviserDbContext(options);

            //Arrange
            var dataSeeder = new DataSeeder(dbContext);
            

            //Act
            dataSeeder.InsertData();

            //Assert
            //Assert.NotNull(result);
            //Assert.NotEqual(result.Id, Guid.Empty);
            //Assert.True(result.IsActive);
            //Assert.True(result.CreatedDate > DateTime.MinValue);
            //Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }
    }
}
