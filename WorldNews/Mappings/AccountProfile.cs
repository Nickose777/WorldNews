using AutoMapper;
using System.Web;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Models;

namespace WorldNews.Mappings
{
    class AccountProfile : Profile
    {
        public AccountProfile()
        {
            this.CreateMap<ProfileBaseDTO, ProfileViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => HttpContext.Current.User.Identity.Name));
        }
    }
}