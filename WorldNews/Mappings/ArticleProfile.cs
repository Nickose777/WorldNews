using AutoMapper;
using System.Web;
using WorldNews.Logic.DTO.Article;
using WorldNews.Models.Article;

namespace WorldNews.Mappings
{
    class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            this.CreateMap<ArticleCreateViewModel, ArticleCreateDTO>()
                .ForSourceMember(src => src.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorLogin, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));
        }
    }
}