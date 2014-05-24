using Logger;
using Core.Interface;
using Ninject.Modules;

namespace Services.DIModule
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<ConsoleLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Fatal);
        }
    }
}
