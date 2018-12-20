using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using PexesoApp.ViewModels;

namespace PexesoApp
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
