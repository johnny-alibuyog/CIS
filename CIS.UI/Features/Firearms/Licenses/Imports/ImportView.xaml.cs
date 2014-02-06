using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Firearms.Licenses.Imports
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl, IViewFor<ImportViewModel>
    {
        #region IViewFor<ImportViewModel>

        public ImportViewModel ViewModel
        {
            get { return this.DataContext as ImportViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ImportView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<ImportViewModel>());
        }
    }
}
