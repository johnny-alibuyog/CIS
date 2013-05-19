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

namespace CIS.UI.Features.Firearms.Maintenances
{
    /// <summary>
    /// Interaction logic for MakeListView.xaml
    /// </summary>
    public partial class MakeListView : UserControl, IViewFor<MakeListViewModel>
    {
        #region IViewFor<MakeListViewModel> Members

        public MakeListViewModel ViewModel
        {
            get { return this.DataContext as MakeListViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public MakeListView()
        {
            InitializeComponent();

            ViewModel = new MakeListViewModel();
        }
    }
}
