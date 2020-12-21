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
                cfg.CreateMap<Post, DTO.Post>()
                    .ForMember(dest => dest.Tags, opt =>
                    {
                        opt.Condition(src => src.PostTags != null && src.PostTags.Count > 0);
                        opt.MapFrom(src => src.PostTags.Select(pt => pt.Tag));
                    }).ReverseMap()
                    .ForMember(dest => dest.PostTags, opt =>
                    {
                        opt.Condition(src => src.Tags != null && src.Tags.Count > 0);
                        opt.MapFrom(src => src.Tags.Select(t => new PostTag { TagId = t.Id, PostId = src.Id }));
                    });
                cfg.CreateMap<Tag, DTO.Tag>().ReverseMap();
                cfg.CreateMap<Comments, DTO.Comments>().ReverseMap();
                cfg.CreateMap<Category, DTO.Category>().ReverseMap();
            });
            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}
