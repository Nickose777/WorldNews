namespace WorldNews.Logic.DTO.Category
{
    public class CategoryListDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int NewsCount { get; set; }

        public bool IsEnabled { get; set; }
    }
}
