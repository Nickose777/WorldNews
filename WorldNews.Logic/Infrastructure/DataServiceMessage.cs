namespace WorldNews.Logic.Infrastructure
{
    public class DataServiceMessage<TData> : ServiceMessage where TData : class
    {
        public TData Data { get; set; }
    }
}
