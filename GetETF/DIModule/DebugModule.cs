using Logger;
using Core.Interface;
using Ninject.Modules;
using Services.Interfaces;
using Services.Services;

namespace GetETF.DIModule
{
    class DebugModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Debug);

            Bind<IETFService>().To<ETFService>().InSingletonScope();
            //Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
