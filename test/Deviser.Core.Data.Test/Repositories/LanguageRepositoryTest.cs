using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Common.DomainTypes;
using Deviser.TestCommon;
using Deviser.WI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Deviser.Core.Data.Test.DataRepositorys
{
    public class LanguageRepositoryTest : TestBase
    {
        [Fact]
        public void CreateLanguageSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var languages = TestDataRepository.GetLanguages();
            var language = languages.First();

            //Act
            var result = languageRepository.CreateLanguage(language);

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
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            Language language = null;

            //Act
            var result = languageRepository.CreateLanguage(language);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLanguagesSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }

            //Act
            var result = languageRepository.GetLanguages();

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
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Language.RemoveRange(dbContext.Language);

            //Act
            var result = languageRepository.GetLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetActiveLanguagesSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }
            var inActiveLang = languages.First();
            inActiveLang.IsActive = false;
            languageRepository.UpdateLanguage(inActiveLang);

            //Act
            var result = languageRepository.GetActiveLanguages();

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
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            dbContext.Language.RemoveRange(dbContext.Language);

            //Act
            var result = languageRepository.GetActiveLanguages();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void GetLanguageSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }
            var id = languages.First().Id;

            //Act
            var result = languageRepository.GetLanguage(id);

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
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }
            var id = Guid.Empty;

            //Act
            var result = languageRepository.GetLanguage(id);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void IsMultilingualSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }

            //Act
            var result = languageRepository.IsMultilingual();

            //Assert            
            Assert.True(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void IsMultilingualFail()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }

            foreach (var lang in languages)
            {
                lang.IsActive = false;
                languageRepository.UpdateLanguage(lang);
            }

            //Act
            var result = languageRepository.IsMultilingual();

            //Assert            
            Assert.True(!result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }

        [Fact]
        public void UpdateLanguageSuccess()
        {
            //Arrange
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }

            var languageToUpdate = languages.First();

            //Act
            languageToUpdate.IsActive = false;
            languageToUpdate.NativeName = "NewName";

            var result = languageRepository.UpdateLanguage(languageToUpdate);

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
            var languageRepository = new LanguageRepository(_container);
            var dbContext = _serviceProvider.GetRequiredService<DeviserDbContext>();
            var languages = TestDataRepository.GetLanguages();
            foreach (var ct in languages)
            {
                languageRepository.CreateLanguage(ct);
            }

            Language languageToUpdate = null;

            //Act
            var result = languageRepository.UpdateLanguage(languageToUpdate);

            //Assert
            Assert.Null(result);

            //Clean
            dbContext.Language.RemoveRange(dbContext.Language);
        }
    }
}
