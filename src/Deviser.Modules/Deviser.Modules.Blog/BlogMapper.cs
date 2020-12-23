using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Deviser.Modules.Blog.Models;

namespace Deviser.Modules.Blog
{
    public class BlogMapper
    {
        public static MapperConfiguration MapperConfiguration;
        public static IMapper Mapper;
        static BlogMapper()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.Blog, DTO.Blog>().ReverseMap();
                cfg.CreateMap<Post, DTO.Post>().ReverseMap();
                cfg.CreateMap<Tag, DTO.Tag>().ReverseMap();
                cfg.CreateMap<Comments, DTO.Comments>().ReverseMap();
                cfg.CreateMap<Category, DTO.Category>().ReverseMap();
            });
            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}
