using Logger;
using Core.Interface;
using Ninject.Modules;

namespace Services.DIModule
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetWrapper>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Info);
        }
    }
}
