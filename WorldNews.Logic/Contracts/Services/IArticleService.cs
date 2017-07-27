using System;
using System.Collections.Generic;
using WorldNews.Logic.DTO.Article;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface IArticleService : IDisposable
    {
        ServiceMessage Create(ArticleCreateDTO articleDTO);

        ServiceMessage Edit(ArticleEditDTO articleDTO);

        ServiceMessage Delete(string id);

        DataServiceMessage<ArticleEditDTO> Get(string id);

        DataServiceMessage<ArticleDetailsDTO> GetDetails(string id);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllByCategory(string categoryName);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllEnabled();

        StructServiceMessage<int> GetPagesCount(int itemsPerPage);

        StructServiceMessage<int> GetPagesCountByCategory(int itemsPerPage, string categoryName);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllByCategoryByPage(int pageNumber, int itemsPerPage, string categoryName);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllEnabledByPage(int pageNumber, int itemsPerPage);

        DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll();

        DataServiceMessage<IEnumerable<ArticleAuthorListDTO>> GetAllWithAuthors();
    }
}
