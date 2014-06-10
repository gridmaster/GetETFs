// -----------------------------------------------------------------------
// <copyright file="ProductionModule.cs" company="Magic FireFly">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Core.Interface;
using Logger;
using Ninject.Modules;
//using Services.Interface;
//using Services.Service;

namespace GetETF.DIModule
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<ConsoleLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Fatal);

            //Bind<IMyFakeService>().To<MyFakeService>().InSingletonScope();
            //Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
