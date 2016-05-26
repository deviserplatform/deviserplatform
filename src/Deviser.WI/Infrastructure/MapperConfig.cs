using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Deviser.WI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using User = Deviser.Core.Data.Entities.User;

namespace Deviser.WI.Infrastructure
{
    public class MapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<DTO.User, User>().ForMember(m => m.Roles, opt => opt.Ignore()).ReverseMap();
                //config.CreateMap<User, DTO.User>().ForMember(m => m.Roles, opt => opt.Ignore());
                config.CreateMap<PageLayout, Layout>().ReverseMap();
                //config.CreateMap<Layout, PageLayout>();
            });
           
        }
    }
}