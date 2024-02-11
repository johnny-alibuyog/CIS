using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Utilities.CommonDialogs;
using Ninject.Modules;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Modules
{
    public class CommonDialogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageBoxService>()
                .To<MessageBoxService>();

            Bind<IOpenDirectoryDialogService>()
                .To<OpenDirectoryDialogService>();

            Bind<IOpenImageDialogService>()
                .To<OpenImageDialogService>();
        }
    }
}
