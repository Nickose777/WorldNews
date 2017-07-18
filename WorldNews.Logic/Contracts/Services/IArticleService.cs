using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Article;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IArticleService : IDisposable
    {
        ServiceMessage Create(ArticleCreateDTO articleDTO);

        DataServiceMessage<ArticleDetailsDTO> Get(string id);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll();
    }
}
