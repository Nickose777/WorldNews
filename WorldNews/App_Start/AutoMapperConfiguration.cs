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
            });
        }
    }
}