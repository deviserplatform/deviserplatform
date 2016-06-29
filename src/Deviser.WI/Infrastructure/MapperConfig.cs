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
                config.CreateMap<Core.Library.DomainTypes.User, User>().ForMember(m => m.Roles, opt => opt.Ignore());
                config.CreateMap<User, Core.Library.DomainTypes.User>().ForMember(m => m.Roles, opt => opt.Ignore());
                config.CreateMap<Core.Library.DomainTypes.PageLayout, Layout>();
                config.CreateMap<Layout, Core.Library.DomainTypes.PageLayout>();
                config.CreateMap<PropertyOptionList, Core.Library.DomainTypes.PropertyOptionList>().ReverseMap();
                config.CreateMap<Property, Core.Library.DomainTypes.Property>().ReverseMap();
                config.CreateMap<Core.Library.DomainTypes.ContentType, ContentType>().ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.ContentTypeProperties.Select(ctp => ctp.Property)))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.ContentDataType.Name));
                config.CreateMap<ContentDataType, Core.Library.DomainTypes.ContentDataType>().ReverseMap();
                config.CreateMap<PageContent, Core.Library.DomainTypes.PageContent>().ReverseMap();

                config.CreateMap<Core.Library.DomainTypes.LayoutType, LayoutType>().ReverseMap()
               .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.LayoutTypeProperties.Select(ctp => ctp.Property)));


            });

        }
    }
}