using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Deviser.Modules.PageManagement.Services;

namespace Deviser.Modules.PageManagement
{
    public static class PageManagementMapper
    {
        public static IMapper Mapper;
        static PageManagementMapper()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<Page, PageViewModel>()
                    .ForMember(p => p.ChildPage, option => option.Ignore())
                    .ReverseMap()
                    .ForMember(p => p.ChildPage, option => option.Ignore());
            });

            Mapper = config.CreateMapper();
        }
    }
}
