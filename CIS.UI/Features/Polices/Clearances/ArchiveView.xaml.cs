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
using ReactiveUI;

namespace CIS.UI.Features.Polices.Clearances
{
    /// <summary>
    /// Interaction logic for ArchiveView.xaml
    /// </summary>
    public partial class ArchiveView : UserControl, IViewFor<ArchiveViewModel>
    {
        #region IViewFor<ArchiveViewModel> Members

        public ArchiveViewModel ViewModel
        {
            get { return this.DataContext as ArchiveViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ArchiveView()
        {
            InitializeComponent();

            ViewModel = new ArchiveViewModel();
        }
    }
}
