using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataProviders
{
    public class LanguageProviderTest : TestBase
    {
        [Fact]
        public void CreateLanguageSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var languages = TestDataProvider.GetLanguages();
            var language = languages.First();

            //Act
            var result = languageProvider.CreateLanguage(language);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(result.IsActive);
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);
        }

        [Fact]
        public void CreateLanguageFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            Language language = null;

            //Act
            var result = languageProvider.CreateLanguage(language);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLanguagesSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }

            //Act
            var result = languageProvider.GetLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void GetLanguagesFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Language.RemoveRange(dbContext.Language);

            //Act
            var result = languageProvider.GetLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetActiveLanguagesSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }
            var inActiveLang = languages.First();
            inActiveLang.IsActive = false;
            languageProvider.UpdateLanguage(inActiveLang);

            //Act
            var result = languageProvider.GetActiveLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.True(result.All(l => l.IsActive));

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void GetActiveLanguagesFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Language.RemoveRange(dbContext.Language);

            //Act
            var result = languageProvider.GetActiveLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLanguageSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }
            var id = languages.First().Id;

            //Act
            var result = languageProvider.GetLanguage(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.True(!string.IsNullOrEmpty(result.EnglishName));
            Assert.True(!string.IsNullOrEmpty(result.NativeName));
            Assert.True(!string.IsNullOrEmpty(result.CultureCode));
            Assert.True(!string.IsNullOrEmpty(result.FallbackCulture));
            Assert.True(result.CreatedDate > DateTime.MinValue);
            Assert.True(result.LastModifiedDate > DateTime.MinValue);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void GetLanguageFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }
            var id = Guid.Empty;

            //Act
            var result = languageProvider.GetLanguage(id);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void IsMultilingualSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }

            //Act
            var result = languageProvider.IsMultilingual();

            //Assert            
            Assert.True(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void IsMultilingualFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }

            foreach (var lang in languages)
            {
                lang.IsActive = false;
                languageProvider.UpdateLanguage(lang);
            }

            //Act
            var result = languageProvider.IsMultilingual();

            //Assert            
            Assert.True(!result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void UpdateLanguageSuccess()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }

            var languageToUpdate = languages.First();

            //Act
            languageToUpdate.IsActive = false;
            languageToUpdate.NativeName = "NewName";

            var result = languageProvider.UpdateLanguage(languageToUpdate);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.NativeName == languageToUpdate.NativeName);
            Assert.True(result.IsActive == languageToUpdate.IsActive);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void UpdateLanguageFail()
        {
            //Arrange
            var languageProvider = new LanguageProvider(container);
            var dbContext = serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataProvider.GetLanguages();
            foreach (var ct in languages)
            {
                languageProvider.CreateLanguage(ct);
            }

            Language languageToUpdate = null;

            //Act
            var result = languageProvider.UpdateLanguage(languageToUpdate);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }
    }
}
