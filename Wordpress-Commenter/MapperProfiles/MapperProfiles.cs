using AppDTOs;
using AutoMapper;
using DataAccess.Models.User;

namespace Wordpress_Commenter.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<CommentDTO, Comment>().ReverseMap();
            CreateMap<ReviewDTO, Review>().ReverseMap();
            CreateMap<WebsiteDTO, Websites>().ReverseMap();
            CreateMap<CommentGroupDTO, CommentGroup>().ReverseMap();
            CreateMap<SendCommentConfigurationDTO, SendCommentConfiguration>().ReverseMap();
        }
    }
}
