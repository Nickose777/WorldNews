namespace WorldNews.Logic.Contracts
{
    public interface IEncryptor
    {
        string Encrypt(string text);

        string Decrypt(string text);
    }
}
