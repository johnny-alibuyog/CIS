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
using ReactiveUI;

namespace CIS.UI.Features.Settings.Appearance
{
    /// <summary>
    /// Interaction logic for SettingsAppearance.xaml
    /// </summary>
    public partial class AppearanceView : UserControl, IViewFor<AppearanceViewModel>
    {
        public AppearanceView()
        {
            InitializeComponent();

            // create and assign the appearance view model
            this.ViewModel = IoC.Container.Resolve<AppearanceViewModel>();
        }

        #region IViewFor<AppearanceViewModel> Members

        public AppearanceViewModel ViewModel
        {
            get { return this.DataContext as AppearanceViewModel; }
            set { this.DataContext = value; }

        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }

        } 

        #endregion
    }
}
