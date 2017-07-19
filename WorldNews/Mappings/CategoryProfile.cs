using AutoMapper;
using WorldNews.Logic.DTO.Category;
using WorldNews.Models.Category;

namespace WorldNews.Mappings
{
    class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<CategoryCreateViewModel, CategoryCreateDTO>();
            this.CreateMap<CategoryListDTO, CategoryListViewModel>();
            this.CreateMap<CategoryEditDTO, CategoryEditViewModel>().ReverseMap();
        }
    }
}