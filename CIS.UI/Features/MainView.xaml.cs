using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Windows.Controls;
using ReactiveUI;
using System.Windows.Media;
using System.Windows;

namespace CIS.UI.Features
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow, IViewFor<MainViewModel>
    {
        #region IViewFor<MainViewModel> Members

        public MainViewModel ViewModel
        {
            get { return this.DataContext as MainViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public MainView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<MainViewModel>());

            var root = this.GetTemplateChild("LayoutRoot");
            //root.LayoutTransform = new ScaleTransform(1.5, 1.5);

            //var root = (FrameworkElement)this.GetVisualChild(0);
            //if (root != null)
            //{
            //    root.LayoutTransform = new ScaleTransform(1.5, 1.5);
            //}
            //this.LayoutTransform = new ScaleTransform(1.5, 1.5);
        }
    }
}
