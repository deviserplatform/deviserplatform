using AutoMapper;
using Deviser.Core.Data.Entities;
using Deviser.Core.Library.DomainTypes;
using Deviser.WI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deviser.WI.Infrastructure
{
    public class MapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<UserDTO, User>();
            Mapper.CreateMap<PageLayout, Layout>();
            Mapper.CreateMap<Layout, PageLayout> ();
        }
    }
}