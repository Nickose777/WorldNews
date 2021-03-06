﻿using Ninject.Modules;
using WorldNews.Core;
using WorldNews.Data.Contracts;
using WorldNews.Data.Infrastructure;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.NinjectModules
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUnitOfWork>().To<UnitOfWork>();
            this.Bind<IEncryptor>().To<Encryptor>();
            this.Bind<WorldNewsDbContext>().ToSelf();
        }
    }
}
