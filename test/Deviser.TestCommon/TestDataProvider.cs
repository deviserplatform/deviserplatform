using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Data.Entities;

namespace Deviser.TestCommon
{
    public static class TestDataProvider
    {

        public static List<ContentDataType> GetContentDataTypes()
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

        public static List<ContentType> GetContentTypes()
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

        public static List<Language> GetLanguages()
        {
            var languages = new List<Language>();

            languages.Add(new Language
            {
                Id = Guid.NewGuid(),
                CultureCode = "en-US",
                EnglishName = "English - United States",
                FallbackCulture = "en-US",
                NativeName = "English - United States"
            });
            languages.Add(new Language
            {
                Id = Guid.NewGuid(),
                CultureCode = "de-CH",
                EnglishName = "German - Switzerland",
                FallbackCulture = "en-US",
                NativeName = "Deutsch - Schweiz"
            });
            languages.Add(new Language
            {
                Id = Guid.NewGuid(),
                CultureCode = "fr-CH",
                EnglishName = "French - Switzerland",
                FallbackCulture = "en-US",
                NativeName = "French - Suisse"
            });

            return languages;
        }

        public static List<Property> GetProperties()
        {
            var properties = new List<Property>();
            properties.Add(new Property()
            {
                Id = Guid.NewGuid(),
                Name = "cssclass",
                Label = "Css Class"
            });
            properties.Add(new Property()
            {
                Id = Guid.NewGuid(),
                Name = "height",
                Label = "Height"
            });
            return properties;
        }

        public static List<Layout> GetLayouts()
        {
            var layouts = new List<Layout>();

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"PropertyOptionListId\":null,\"PropertyOptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
                Name = "Home Page Layout"
            });

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"PropertyOptionListId\":null,\"PropertyOptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
                Name = "Admin Layout"
            });

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"PropertyOptionListId\":null,\"PropertyOptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
                Name = "Inner Layout"
            });

            return layouts;
        }

        public static List<LayoutType> GetLayoutTypes()
        {
            var layoutTypes = new List<LayoutType>();

            layoutTypes.Add(new LayoutType
            {
                Id = Guid.NewGuid(),
                Name = "container",                
                Label ="Container",
                LayoutTypeIds = $"{Guid.NewGuid()},{Guid.NewGuid()}",
                IconClass = "fa fa-square-o"
            });
            layoutTypes.Add(new LayoutType
            {
                Id = Guid.NewGuid(),
                Name = "row",
                Label = "Row",
                LayoutTypeIds = $"{Guid.NewGuid()},{Guid.NewGuid()}",
                IconClass = "fa fa-align-justify"
            });
            layoutTypes.Add(new LayoutType
            {
                Id = Guid.NewGuid(),
                Name = "column",
                Label = "Column",
                LayoutTypeIds = $"{Guid.NewGuid()},{Guid.NewGuid()}",
                IconClass = "fa fa-columns"
            });

            return layoutTypes;
        }
    }
}
