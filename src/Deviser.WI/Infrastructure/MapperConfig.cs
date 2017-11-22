using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.WI.Infrastructure
{
    public class MapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(config =>
            {               
                config.CreateMap<ContentDataType, Core.Common.DomainTypes.ContentDataType>().ReverseMap();
                config.CreateMap<ContentPermission, Core.Common.DomainTypes.ContentPermission>().ReverseMap();

                config.CreateMap<Core.Common.DomainTypes.ContentType, ContentType>()
                .ForMember(dest => dest.ContentDataTypeId, opt => opt.MapFrom(src => src.ContentDataType.Id))
                .ForMember(dest => dest.ContentTypeProperties, opt => opt.MapFrom(src => src.Properties.Select(ctp => new ContentTypeProperty
                {
                    PropertyId = ctp.Id,
                    ConentTypeId = src.Id,
                    Property = Mapper.Map<Property>(ctp)
                }
                )))
                .ForMember(dest => dest.ContentTypeProperties, opt => opt.Condition(src => src.Properties != null))
                .ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.ContentTypeProperties.Select(ctp => ctp.Property)))
                .ForMember(dest => dest.Properties, opt => opt.Condition(src => src.ContentTypeProperties != null && src.ContentTypeProperties.All(cp => cp.Property != null)))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src =>
                   src.ContentDataType != null ? src.ContentDataType.Name : null));
                

                config.CreateMap<Core.Common.DomainTypes.LayoutType, LayoutType>()
                .ForMember(dest => dest.LayoutTypeProperties, opt => opt.MapFrom(src =>
                    src.Properties != null ? src.Properties.Select(ctp => new LayoutTypeProperty { PropertyId = ctp.Id, LayoutTypeId = src.Id }) : null))
                .ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src =>
                  src.LayoutTypeProperties != null ? src.LayoutTypeProperties.Select(ctp => ctp.Property) : null));

                config.CreateMap<Module, Core.Common.DomainTypes.Module>().ReverseMap();
                config.CreateMap<ModuleAction, Core.Common.DomainTypes.ModuleAction>().ReverseMap();
                config.CreateMap<ModuleActionType, Core.Common.DomainTypes.ModuleActionType>().ReverseMap();
                config.CreateMap<ModulePermission, Core.Common.DomainTypes.ModulePermission>().ReverseMap();

                config.CreateMap<PageContent, Core.Common.DomainTypes.PageContent>()
               .ForMember(dest => dest.Properties, opt =>
               opt.MapFrom(src => !string.IsNullOrEmpty(src.Properties) ? SDJsonConvert.DeserializeObject<List<Core.Common.DomainTypes.Property>>(src.Properties) : null))
               .ReverseMap()
               .ForMember(dest => dest.Properties, opt =>
               opt.MapFrom(src => (src.Properties != null) ? SDJsonConvert.SerializeObject(src.Properties) : null));

                config.CreateMap<Language, Core.Common.DomainTypes.Language>().ReverseMap();
                config.CreateMap<Layout, Core.Common.DomainTypes.Layout>().ReverseMap();
                config.CreateMap<Core.Common.DomainTypes.Layout, Core.Common.DomainTypes.PageLayout>().ReverseMap();
                config.CreateMap<Page, Core.Common.DomainTypes.Page>()
                    .MaxDepth(10)
                    .ReverseMap()
                    .MaxDepth(10);

                config.CreateMap<PageContentTranslation, Core.Common.DomainTypes.PageContentTranslation>().ReverseMap();
                config.CreateMap<PageModule, Core.Common.DomainTypes.PageModule>().ReverseMap();
                config.CreateMap<PagePermission, Core.Common.DomainTypes.PagePermission>()
                    .ReverseMap();
                //.ForMember(dest => dest.Page, opt => opt.Ignore());
                config.CreateMap<PageTranslation, Core.Common.DomainTypes.PageTranslation>()
                    .ReverseMap();
                //.ForMember(dest => dest.Page, opt => opt.Ignore());

                config.CreateMap<Permission, Core.Common.DomainTypes.Permission>().ReverseMap();
                config.CreateMap<Property, Core.Common.DomainTypes.Property>().ReverseMap();

                config.CreateMap<OptionList, Core.Common.DomainTypes.OptionList>()
                .ForMember(dest => dest.List, opt =>
                opt.MapFrom(src => !string.IsNullOrEmpty(src.List) ? SDJsonConvert.DeserializeObject<List<Core.Common.DomainTypes.PropertyOption>>(src.List) : null))
                .ReverseMap()
                .ForMember(dest => dest.List, opt =>
                opt.MapFrom(src => (src.List != null) ? SDJsonConvert.SerializeObject(src.List) : null));

                config.CreateMap<Role, Core.Common.DomainTypes.Role>().ReverseMap();

                //Roles from db needs to be ignored, because User.Roles is not Role type, it is UserRole - join entity.
                config.CreateMap<User, Core.Common.DomainTypes.User>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

                config.CreateMap<SiteSetting, Core.Common.DomainTypes.SiteSetting>().ReverseMap();

            });

        }
    }
}