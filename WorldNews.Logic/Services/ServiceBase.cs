using System;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;

namespace WorldNews.Logic.Services
{
    public abstract class ServiceBase : IDisposable
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IEncryptor encryptor;

        public ServiceBase(IUnitOfWork unitOfWork, IEncryptor encryptor)
        {
            this.unitOfWork = unitOfWork;
            this.encryptor = encryptor;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
