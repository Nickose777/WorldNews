using AutoMapper;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Models;

namespace WorldNews.Mappings
{
    class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            this.CreateMap<RegisterViewModel, UserRegisterDTO>()
                .ForSourceMember(src => src.ConfirmPassword, opt => opt.Ignore());
            this.CreateMap<ModeratorRegisterViewModel, ModeratorRegisterDTO>()
                .ForSourceMember(src => src.ConfirmPassword, opt => opt.Ignore())
                .ForSourceMember(src => src.Photo, opt => opt.Ignore());
        }
    }
}