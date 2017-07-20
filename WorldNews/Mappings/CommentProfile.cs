using AutoMapper;
using System.Web;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Models.Comment;

namespace WorldNews.Mappings
{
    class CommentProfile : Profile
    {
        public CommentProfile()
        {
            this.CreateMap<CommentCreateViewModel, CommentCreateDTO>()
                .ForMember(dest => dest.AuthorLogin, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));
            this.CreateMap<CommentListDTO, CommentListViewModel>();
            this.CreateMap<CommentBanViewModel, CommentBanDTO>()
                .ForMember(dest => dest.ModeratorLogin, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));
            this.CreateMap<CommentBanDetailsDTO, CommentBanDetailsViewModel>();
        }
    }
}