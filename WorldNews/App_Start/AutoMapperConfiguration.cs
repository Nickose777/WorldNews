using AutoMapper;
using WorldNews.Mappings;

namespace WorldNews
{
    static class AutoMapperConfiguration
    {
        public static void InitializeAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<RegisterProfile>();
                config.AddProfile<AccountProfile>();
                config.AddProfile<CategoryProfile>();
                config.AddProfile<ArticleProfile>();
            });
        }
    }
}