using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Logic.DTO.Category;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface ICategoryService : IDisposable
    {
        ServiceMessage Create(CategoryCreateDTO categoryDTO);
    }
}
