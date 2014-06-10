// -----------------------------------------------------------------------
// <copyright file="DesignTimeModule.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Core.Interface;
using Logger;
//using Services.Interface;
//using Services.Service;
using Ninject.Modules;

namespace GetETF.DIModule
{
    public class DesignTimeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetWrapper>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Info);

            //Bind<IMyFakeService>().To<MyFakeService>().InSingletonScope();
            //Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
