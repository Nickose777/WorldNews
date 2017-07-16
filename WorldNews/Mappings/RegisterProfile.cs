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
        }
    }
}