﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using DIContainer;
using GetETF.DIModule;
using Logger;
using Ninject;
using Services.Interfaces;

namespace GetETF
{
    class Program
    {
        #region initialize DI container
        // This initializes the IOC Container and implements
        // the singleton pattern.
        private static void InitializeDiContainer()
        {
            NinjectSettings settings = new NinjectSettings
            {
                LoadExtensions = false
            };

            // change DesignTimeModule to run other implementation ProductionModule || DebugModule
            IOCContainer.Instance.Initialize(settings, new DebugModule());
        }
        #endregion Initialize DI Container

        static void Main(string[] args)
        {
            try
            {
                InitializeDiContainer();

                DIContainer.IOCContainer.Instance.Get<ILogger>()
                    .InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);
                DIContainer.IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's runnin'...{0}", Environment.NewLine);
                DIContainer.IOCContainer.Instance.Get<IETFService>().DoSomething(123);
            }
            catch (Exception exc)
            {
                DIContainer.IOCContainer.Instance.Get<ILogger>().Fatal("Sucker blew up: {0}", exc);
            }
            finally
            {
                DIContainer.IOCContainer.Instance.Get<ILogger>().InfoFormat("{0}Main's done'...{0}", Environment.NewLine);
                DIContainer.IOCContainer.Instance.Get<ILogger>()
                    .InfoFormat("{0}********************************************************************************{0}", Environment.NewLine);

                Console.ReadKey();
            }
        }
    }
}
