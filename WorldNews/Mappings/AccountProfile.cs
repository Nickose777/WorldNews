using System.Web;
using WorldNews.Logic.DTO.Account;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Models;
using WorldNews.Models.Account;
using WorldNews.Models.Profile;

namespace WorldNews.Mappings
{
    class AccountProfile : ProfileBase
    {
        public AccountProfile()
        {
            this.CreateMap<ProfileBaseDTO, ProfileViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));

            this.CreateMap<ModeratorListDTO, ModeratorListViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => GetServerPhotoPath(src.PhotoLink)));
            this.CreateMap<ModeratorDetailsDTO, ModeratorDetailsViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => GetServerPhotoPath(src.PhotoLink)));
            this.CreateMap<ModeratorEditDTO, ModeratorEditViewModel>()
                .ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src => GetServerPhotoPath(src.PhotoLink)))
                .ReverseMap();

            this.CreateMap<ChangePasswordViewModel, ChangePasswordDTO>()
                .ForSourceMember(src => src.Submit, opt => opt.Ignore())
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));
        }
    }
}