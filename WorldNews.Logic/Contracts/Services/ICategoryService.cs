using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Category;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface ICategoryService : IDisposable
    {
        ServiceMessage Create(CategoryCreateDTO categoryDTO);

        DataServiceMessage<IEnumerable<string>> GetAllNames();

        DataServiceMessage<IEnumerable<CategoryListDTO>> GetAll();
    }
}
