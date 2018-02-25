using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.TestCommon
{
    public static class TestDataProvider
    {

        //public static List<ContentDataType> GetContentDataTypes()
        //{
        //    var contentDataTypes = new List<ContentDataType>();
        //    var stringType = new ContentDataType
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "string",
        //        Label = "string"
        //    };
        //    var objectType = new ContentDataType
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "object",
        //        Label = "object"
        //    };
        //    var arrayType = new ContentDataType
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "array",
        //        Label = "array"
        //    };

        //    contentDataTypes.Add(stringType);
        //    contentDataTypes.Add(objectType);
        //    contentDataTypes.Add(arrayType);
        //    return contentDataTypes;
        //}
        
        public static List<ContentPermission> GetContentPermissions()
        {
            var contentPermissions = new List<ContentPermission>();
            contentPermissions.Add(new ContentPermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });

            contentPermissions.Add(new ContentPermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
            });
            return contentPermissions;

        }

        public static List<ContentType> GetContentTypes()
        {
            var contentTypes = new List<ContentType>();
            //var properties = GetProperties();
            //var stringType = new ContentDataType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "string",
            //    Label = "string"
            //};
            //var objectType = new ContentDataType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "object",
            //    Label = "object"
            //};
            //var arrayType = new ContentDataType
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "array",
            //    Label = "array"
            //};

            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Text",
                Label = "Text",
                //ContentDataType = arrayType,
                Properties = GetProperties()
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "Image",
                Label = "Image",
                //ContentDataType = objectType,
                Properties = GetProperties()
            });
            contentTypes.Add(new ContentType
            {
                Id = Guid.NewGuid(),
                Name = "RichText",
                Label = "Rich text",
                //ContentDataType = stringType,
                Properties = GetProperties()
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

        public static List<Layout> GetLayouts()
        {
            var layouts = new List<Layout>();

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
                Name = "Home Page Layout"
            });

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
                Name = "Admin Layout"
            });

            layouts.Add(new Layout
            {
                Id = Guid.NewGuid(),
                Config = "[{\"Id\":\"0fcf04a2 - 3d71 - 26b0 - c371 - 6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8 - 4afe - ad4a - a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31 - 778b - 45dd - bd33 - eeb2a088d2bc\",\"Name\":\"cssclass\",\"Label\":\"Css Class\",\"Value\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true}],\"PlaceHolders\":[]}]",
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
                Label = "Container",
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

        public static List<Module> GetModules()
        {
            var modules = new List<Module>();
            var loginActions = new List<ModuleAction>();
            var registerActions = new List<ModuleAction>();
            var viewType = new ModuleActionType
            {
                Id = Guid.Parse("72366792-3740-4E6B-B960-9C9C5334163A"),
                ControlType = "View"
            };

            var editType = new ModuleActionType
            {
                Id = Guid.Parse("192278B6-7BF2-40C2-A776-B9CA5FB04FBB"),
                ControlType = "Edit"
            };

            loginActions.Add(new ModuleAction()
            {
                Id = Guid.NewGuid(),
                ActionName = "Login",
                ControllerName = "Account",
                ControllerNamespace = "Deviser.Modules.Security.Controllers",
                DisplayName = "Login",
                IconClass = "fa fa-sign-in",
                ModuleActionType = viewType
            });

            loginActions.Add(new ModuleAction()
            {
                Id = Guid.NewGuid(),
                ActionName = "Index",
                ControllerName = "Edit",
                ControllerNamespace = "Deviser.Modules.Security.Controllers",
                DisplayName = "Login Edit",
                IconClass = "fa fa-sign-in",
                ModuleActionType = editType
            });

            registerActions.Add(new ModuleAction()
            {
                Id = Guid.NewGuid(),
                ActionName = "Index",
                ControllerName = "Home",
                ControllerNamespace = "Deviser.Modules.Language.Controllers",
                DisplayName = "Register",
                IconClass = "fa fa-language",
                ModuleActionType = viewType
            });


            modules.Add(new Module()
            {
                Id = Guid.NewGuid(),
                Name = "Security",
                Label = "Security",
                Description = "Security module",
                Version = "00.00.01",
                ModuleAction = loginActions,
            });

            modules.Add(new Module()
            {
                Id = Guid.NewGuid(),
                Name = "Language",
                Label = "Language",
                Description = "Language module",
                Version = "00.00.01",
                ModuleAction = registerActions
            });
            return modules;
        }

        public static List<ModuleAction> GetModuleActions()
        {
            var moduleActions = new List<ModuleAction>();
            var viewType = new ModuleActionType
            {
                Id = Guid.Parse("72366792-3740-4E6B-B960-9C9C5334163A"),
                ControlType = "View"
            };

            var editType = new ModuleActionType
            {
                Id = Guid.Parse("192278B6-7BF2-40C2-A776-B9CA5FB04FBB"),
                ControlType = "Edit"
            };

            moduleActions.Add(new ModuleAction()
            {
                Id = Guid.NewGuid(),
                ActionName = "Login",
                ControllerName = "Account",
                ControllerNamespace = "Deviser.Modules.Security.Controllers",
                DisplayName = "Login",
                IconClass = "fa fa-sign-in",
                ModuleActionType = viewType

            });

            moduleActions.Add(new ModuleAction()
            {
                Id = Guid.NewGuid(),
                ActionName = "Index",
                ControllerName = "Home",
                ControllerNamespace = "Deviser.Modules.Language.Controllers",
                DisplayName = "Register",
                IconClass = "fa fa-language",
                ModuleActionType = editType

            });

            return moduleActions;
        }

        public static List<ModulePermission> GetModulePermissions()
        {
            var pagePermissions = new List<ModulePermission>();
            var pageModuleId = Guid.NewGuid();
            pagePermissions.Add(new ModulePermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageModuleId = pageModuleId
            });

            pagePermissions.Add(new ModulePermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageModuleId = pageModuleId
            });
            return pagePermissions;
        }

        public static List<Page> GetPages()
        {
            var pages = new List<Page>();
            var pageId1 = Guid.NewGuid();
            var pageId2 = Guid.NewGuid();
            var pageId3 = Guid.NewGuid();

            pages.Add(new Page
            {
                Id = pageId1,
                IsIncludedInMenu = true,
                IsSystem = false,
                PageTranslation = new List<PageTranslation>()
                {
                    new PageTranslation
                    {   
                        Name = "TestPage",
                        Description = "Test Description",
                        Locale = "en-US",
                        Title = "Test Page",
                        URL ="TestPage",
                        //PageId = pageId1
                    }
                },
                PagePermissions = new List<PagePermission>()
                {
                    new PagePermission
                    {
                        Id = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                        //PageId = pageId1
                    }
                }
            });

            pages.Add(new Page
            {
                Id = pageId2,
                IsIncludedInMenu = true,
                IsSystem = false,
                PageTranslation = new List<PageTranslation>()
                {
                    new PageTranslation
                    {
                        Name = "TestPage1",
                        Description = "Test Description1",
                        Locale = "en-US",
                        Title = "Test Page1",
                        URL ="TestPage1",
                        //PageId = pageId2
                    }
                },
                PagePermissions = new List<PagePermission>()
                {
                    new PagePermission
                    {
                        Id = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                        //PageId = pageId2
                    }
                }
            });

            pages.Add(new Page
            {
                Id = pageId3,
                IsIncludedInMenu = true,
                IsSystem = false,
                PageTranslation = new List<PageTranslation>()
                {
                    new PageTranslation
                    {
                        Name = "TestPage2",
                        Description = "Test Description2",
                        Locale = "en-US",
                        Title = "Test Page2",
                        URL ="TestPage2",
                        //PageId = pageId3
                    }
                },
                PagePermissions = new List<PagePermission>()
                {
                    new PagePermission
                    {
                        Id = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                        //PageId = pageId3
                    }
                }
            });

            return pages;
        }

        public static List<PageModule> GetPageModules()
        {

            var pageModules = new List<PageModule>();
            var modules = GetModules();
            var module = modules.First();
            var pageId = Guid.NewGuid();

            pageModules.Add(new PageModule
            {
                Id = Guid.NewGuid(),
                PageId = pageId,
                ContainerId = Guid.NewGuid(),                
                InheritViewPermissions = true,
                InheritEditPermissions = true,
                Module = module,
                ModuleId = module.Id,
                ModuleAction = module.ModuleAction.First(),
                
            ModulePermissions = new List<ModulePermission>()
                {
                    new ModulePermission
                    {
                        Id = Guid.NewGuid(),
                        PageModuleId = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                    }
                },
                IsDeleted = false
            });

            pageModules.Add(new PageModule
            {
                Id = Guid.NewGuid(),
                PageId = pageId,
                ContainerId = Guid.NewGuid(),
                InheritViewPermissions = true,
                InheritEditPermissions = true,
                Module = module,
                ModuleId = module.Id,
                ModuleAction = module.ModuleAction.First(),
                ModulePermissions = new List<ModulePermission>()
                {
                    new ModulePermission
                    {
                        Id = Guid.NewGuid(),
                        PageModuleId = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                    }
                },
                IsDeleted = false
            });

            pageModules.Add(new PageModule
            {
                Id = Guid.NewGuid(),
                PageId = pageId,
                ContainerId = Guid.NewGuid(),
                InheritViewPermissions = true,
                InheritEditPermissions = true,
                Module = module,
                ModuleId = module.Id,
                ModuleAction = module.ModuleAction.First(),
                ModulePermissions = new List<ModulePermission>()
                {
                    new ModulePermission
                    {
                        Id = Guid.NewGuid(),
                        PageModuleId = Guid.NewGuid(),
                        PermissionId = Guid.NewGuid(),
                        RoleId = Guid.NewGuid(),
                    }
                },
                IsDeleted = false
            });

            return pageModules;

        }

        public static List<PageContent> GetPageContents(bool includeChild = true)
        {
            var pageContents = new List<PageContent>();

            var containerId = Guid.NewGuid();
            var pageId = Guid.NewGuid();

            pageContents.Add(new PageContent
            {
                Id = Guid.NewGuid(),
                PageContentTranslation = (includeChild) ? GetPageContentTranslations() : null,
                ContainerId = containerId,
                PageId = pageId,
                ContentType = GetContentTypes().First(),
                Properties = GetProperties(),
                SortOrder = 1,
                ContentPermissions = (includeChild) ? GetContentPermissions() : null,
            });

            pageContents.Add(new PageContent
            {
                Id = Guid.NewGuid(),
                PageContentTranslation = (includeChild) ? GetPageContentTranslations() : null,
                ContainerId = containerId,
                PageId = pageId,
                ContentType = GetContentTypes().First(),
                Properties = GetProperties(),
                SortOrder = 2,
                ContentPermissions = (includeChild) ? GetContentPermissions() : null,
            });

            pageContents.Add(new PageContent
            {
                Id = Guid.NewGuid(),
                PageContentTranslation = (includeChild) ? GetPageContentTranslations() : null,
                ContainerId = containerId,
                PageId = pageId,
                ContentType = GetContentTypes().First(),
                Properties = GetProperties(),
                SortOrder = 3,
                ContentPermissions = (includeChild) ? GetContentPermissions() : null,
            });

            return pageContents;
        }

        public static List<PageContentTranslation> GetPageContentTranslations()
        {
            var pageContentTranslations = new List<PageContentTranslation>();
            pageContentTranslations.Add(new PageContentTranslation
            {
                Id = Guid.NewGuid(),
                ContentData = "Sample data",
                CultureCode = "en-US"
            });
            pageContentTranslations.Add(new PageContentTranslation
            {
                Id = Guid.NewGuid(),
                ContentData = "Sample data1",
                CultureCode = "en-US"
            });
            pageContentTranslations.Add(new PageContentTranslation
            {
                Id = Guid.NewGuid(),
                ContentData = "Sample data2",
                CultureCode = "en-US"
            });
            return pageContentTranslations;
        }

        public static List<PagePermission> GetPagePermissions()
        {
            var pagePermissions = new List<PagePermission>();
            var pageId = Guid.NewGuid();
            pagePermissions.Add(new PagePermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageId = pageId
            });

            pagePermissions.Add(new PagePermission
            {
                PermissionId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                PageId = pageId
            });
            return pagePermissions;
        }

        public static List<Property> GetProperties()
        {
            var properties = new List<Property>();
            
            properties.Add(new Property()
            {
                Id = Guid.NewGuid(),
                Name = "cssclass",
                Label = "Css Class",
                Value="TEST VALUE1",
                OptionList = new OptionList
                {
                    Id = Guid.NewGuid(),
                    Name = "Test List",
                    Label = "Test List"
                }
            });
            properties.Add(new Property()
            {
                Id = Guid.NewGuid(),
                Name = "height",
                Label = "Height",
                Value = "TEST VALUE2",
                OptionList = new OptionList
                {
                    Id = Guid.NewGuid(),
                    Name = "Test List1",
                    Label = "Test List1"
                }
            });
            return properties;
        }

        public static List<PropertyOption> GetOptions()
        {
            var propertyOptions = new List<PropertyOption>();

            propertyOptions.Add(new PropertyOption
            {
                Id = Guid.NewGuid(),
                Name = "Yes",
                Label = "Yes",
            });

            propertyOptions.Add(new PropertyOption
            {
                Id = Guid.NewGuid(),
                Name = "No",
                Label = "No",
            });

            return propertyOptions;
        }

        public static List<OptionList> GetOptionLists()
        {
            var optionList = new List<OptionList>();
            var options = GetOptions();

            optionList.Add(new OptionList
            {
                Id = Guid.NewGuid(),
                Name = "YesNo",
                Label = "Yes No",
                List = options
            });

            optionList.Add(new OptionList
            {
                Id = Guid.NewGuid(),
                Name = "YesNoNa",
                Label = "Yes No Na",
                List = options
            });

            return optionList;
        }

        public static List<Role> GetRoles()
        {
            var roles = new List<Role>();
            roles.Add(new Role()
            {
                Id = Guid.NewGuid(),
                Name = "AllUsers"
            });
            roles.Add(new Role()
            {
                Id = Guid.NewGuid(),
                Name = "RegisteredUsers"
            });
            roles.Add(new Role()
            {
                Id = Guid.NewGuid(),
                Name = "Administrators"
            });
            return roles;
        }

        public static List<User> GetUsers()
        {
            var roles = new List<User>();
            roles.Add(new User()
            {
                Id = Guid.NewGuid(),
                Email = "user1@email.com"
            });
            roles.Add(new User()
            {
                Id = Guid.NewGuid(),
                Email = "user2@email.com"
            });
            roles.Add(new User()
            {
                Id = Guid.NewGuid(),
                Email = "user3@email.com"
            });
            return roles;
        }
    }
}
