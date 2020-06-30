using AutoMapper;
using Deviser.Core.Common;
using Deviser.Core.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Web.Infrastructure
{
    public class DeviserMapperProfile : Profile
    {
        public DeviserMapperProfile()
        {

            CreateMap<AdminPage, Core.Common.DomainTypes.AdminPage>()
            .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name))
            .ForMember(dest => dest.ModuleName, opt => opt.Condition(src => src.Module != null))
            .ReverseMap()
            .ForMember(dest => dest.Module, opt => opt.Ignore());

            CreateMap<ContentPermission, Core.Common.DomainTypes.ContentPermission>().ReverseMap();

            CreateMap<Property, Core.Common.DomainTypes.Property>().ReverseMap();

            CreateMap<ContentFieldType, Core.Common.DomainTypes.ContentFieldType>().ReverseMap();

            CreateMap<Core.Common.DomainTypes.ContentType, ContentType>()
                .ForMember(dest => dest.ContentTypeProperties, opt =>
                    opt.MapFrom(src => src.Properties.Select(ctp =>
                        new ContentTypeProperty
                        {
                            PropertyId = ctp.Id,
                            ContentTypeId = src.Id
                        })))
                .ForMember(dest => dest.ContentTypeProperties, opt => opt.Condition(src => src.Properties != null))
                
                .ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.ContentTypeProperties.Select(ctp => ctp.Property)))
                .ForMember(dest => dest.Properties, opt => opt.Condition(src => src.ContentTypeProperties != null && src.ContentTypeProperties.All(cp => cp.Property != null)));

            CreateMap<ContentTypeField, Core.Common.DomainTypes.ContentTypeField>().ReverseMap();

            CreateMap<Core.Common.DomainTypes.LayoutType, LayoutType>()
            .ForMember(dest => dest.LayoutTypeProperties, opt => opt.MapFrom(src =>
                src.Properties != null ? src.Properties.Select(ctp => new LayoutTypeProperty { PropertyId = ctp.Id, LayoutTypeId = src.Id }) : null))
            .ReverseMap()
            .ForMember(dest => dest.Properties, opt => opt.MapFrom(src =>
              src.LayoutTypeProperties != null ? src.LayoutTypeProperties.Select(ctp => ctp.Property) : null));

            CreateMap<Core.Common.DomainTypes.Module, Module>().ReverseMap();

            CreateMap<ModuleView, Core.Common.DomainTypes.ModuleView>()
            .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.ModuleViewProperties != null ? src.ModuleViewProperties.Select(ctp => ctp.Property) : null))
            .ReverseMap()
            .ForMember(dest => dest.ModuleViewProperties, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Select(macp => new ModuleViewProperty { PropertyId = macp.Id, ModuleViewId = src.Id }) : null));

            //CreateMap<Core.Common.DomainTypes.Module, Module>()
            // .ForMember(dest => dest.ModuleProperties, opt => opt.MapFrom(src =>
            //    src.Properties != null ? src.Properties.Select(ctp => new ModuleProperty { PropertyId = ctp.Id, ModuleId = src.Id }) : null))
            //.ReverseMap()
            //.ForMember(dest => dest.Properties, opt => opt.MapFrom(src =>
            //  src.ModuleProperties != null ? src.ModuleProperties.Select(ctp => ctp.Property) : null));

            CreateMap<ModuleViewType, Core.Common.DomainTypes.ModuleViewType>().ReverseMap();
            CreateMap<ModulePermission, Core.Common.DomainTypes.ModulePermission>().ReverseMap();

            CreateMap<PageContent, Core.Common.DomainTypes.PageContent>()
           .ForMember(dest => dest.Properties, opt =>
           opt.MapFrom(src => !string.IsNullOrEmpty(src.Properties) ? SDJsonConvert.DeserializeObject<List<Core.Common.DomainTypes.Property>>(src.Properties) : null))
           .ReverseMap()
           .ForMember(dest => dest.Properties, opt =>
           opt.MapFrom(src => (src.Properties != null) ? SDJsonConvert.SerializeObject(src.Properties) : null));

            CreateMap<Language, Core.Common.DomainTypes.Language>().ReverseMap();
            CreateMap<Layout, Core.Common.DomainTypes.Layout>().ReverseMap();
            CreateMap<Core.Common.DomainTypes.Layout, Core.Common.DomainTypes.PageLayout>().ReverseMap();
            CreateMap<Page, Core.Common.DomainTypes.Page>()
                .MaxDepth(10)
                .ReverseMap()
                .MaxDepth(10);

            CreateMap<PageType, Core.Common.DomainTypes.PageType>().ReverseMap();

            CreateMap<PageContentTranslation, Core.Common.DomainTypes.PageContentTranslation>().ReverseMap();

            CreateMap<PageModule, Core.Common.DomainTypes.PageModule>()
            .ForMember(dest => dest.Properties, opt =>
           opt.MapFrom(src => !string.IsNullOrEmpty(src.Properties) ? SDJsonConvert.DeserializeObject<ICollection<Core.Common.DomainTypes.Property>>(src.Properties) : null))
           .ReverseMap()
           .ForMember(dest => dest.Properties, opt =>
           opt.MapFrom(src => (src.Properties != null) ? SDJsonConvert.SerializeObject(src.Properties) : null));


            CreateMap<PagePermission, Core.Common.DomainTypes.PagePermission>()
                .ReverseMap();

            CreateMap<PageTranslation, Core.Common.DomainTypes.PageTranslation>()
                .ReverseMap();


            CreateMap<Permission, Core.Common.DomainTypes.Permission>().ReverseMap();


            CreateMap<OptionList, Core.Common.DomainTypes.OptionList>()
            .ForMember(dest => dest.List, opt =>
            opt.MapFrom(src => !string.IsNullOrEmpty(src.List) ? SDJsonConvert.DeserializeObject<List<Core.Common.DomainTypes.PropertyOption>>(src.List) : null))
            .ReverseMap()
            .ForMember(dest => dest.List, opt =>
            opt.MapFrom(src => (src.List != null) ? SDJsonConvert.SerializeObject(src.List) : null));

            CreateMap<Role, Core.Common.DomainTypes.Role>().ReverseMap();

            //Roles from db needs to be ignored, because User.Roles is not Role type, it is UserRole - join entity.
            CreateMap<User, Core.Common.DomainTypes.User>()
            .ForMember(dest => dest.Roles, opt => opt
                .MapFrom(src => src.UserRoles != null ? src.UserRoles.Select(ur => new Core.Common.DomainTypes.Role()
                { Id = ur.RoleId, Name = ur.Role.Name }).ToList() : null))
            .ReverseMap()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
                src.Roles != null ? src.Roles.Select(r => new UserRole() { RoleId = r.Id, UserId = src.Id }).ToList() : null));

            CreateMap<SiteSetting, Core.Common.DomainTypes.SiteSetting>().ReverseMap();
        }
    }
}