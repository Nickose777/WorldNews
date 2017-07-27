namespace WorldNews.Logic.Infrastructure
{
    public class StructServiceMessage<TStruct> : ServiceMessage where TStruct : struct
    {
        public TStruct Data { get; set; }
    }
}
