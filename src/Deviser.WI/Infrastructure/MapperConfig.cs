using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Deviser.WI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User = Deviser.Core.Data.Entities.User;

namespace Deviser.WI.Infrastructure
{
    public class MapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<DTO.User, User>().ForMember(m => m.Roles, opt => opt.Ignore());
            Mapper.CreateMap<User, DTO.User>().ForMember(m => m.Roles, opt => opt.Ignore());
            Mapper.CreateMap<PageLayout, Layout>();
            Mapper.CreateMap<Layout, PageLayout>();
        }
    }
}