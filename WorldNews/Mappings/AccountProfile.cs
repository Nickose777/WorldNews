using AutoMapper;
using System.IO;
using System.Web;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Models;
using WorldNews.Models.Profile;

namespace WorldNews.Mappings
{
    class AccountProfile : Profile
    {
        public AccountProfile()
        {
            this.CreateMap<ProfileBaseDTO, ProfileViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));

            this.CreateMap<ModeratorListDTO, ModeratorListViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => Path.Combine("~/Images/Uploads", System.IO.Path.GetFileName(src.PhotoLink))));

            this.CreateMap<ModeratorDetailsDTO, ModeratorDetailsViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => Path.Combine("~/Images/Uploads", System.IO.Path.GetFileName(src.PhotoLink))));
        }
    }
}