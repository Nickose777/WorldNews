using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace WorldNews.Mappings
{
    public static class AutoMapperExtensions
    {
        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return source.Select(src => Mapper.Map<TSource, TDestination>(src));
        }
    }
}