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
                config.CreateMap<Core.Common.DomainTypes.User, User>().ForMember(m => m.Roles, opt => opt.Ignore());
                config.CreateMap<User, Core.Common.DomainTypes.User>().ForMember(m => m.Roles, opt => opt.Ignore());
                config.CreateMap<Core.Common.DomainTypes.PageLayout, Layout>();
                config.CreateMap<Layout, Core.Common.DomainTypes.PageLayout>();
                config.CreateMap<PropertyOptionList, Core.Common.DomainTypes.PropertyOptionList>().ReverseMap();
                config.CreateMap<Property, Core.Common.DomainTypes.Property>().ReverseMap();
                config.CreateMap<Core.Common.DomainTypes.ContentType, ContentType>().ReverseMap()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.ContentTypeProperties.Select(ctp => ctp.Property)))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.ContentDataType.Name));
                config.CreateMap<ContentDataType, Core.Common.DomainTypes.ContentDataType>().ReverseMap();
                config.CreateMap<PageContent, Core.Common.DomainTypes.PageContent>().ReverseMap();

                config.CreateMap<Core.Common.DomainTypes.LayoutType, LayoutType>().ReverseMap()
               .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.LayoutTypeProperties.Select(ctp => ctp.Property)));


            });

        }
    }
}