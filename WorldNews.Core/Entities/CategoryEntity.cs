using System.Collections.Generic;

namespace WorldNews.Core.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public virtual ICollection<ArticleEntity> Articles { get; set; }
    }
}
