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
using System.Windows.Shapes;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Blotters.MasterList
{
    /// <summary>
    /// Interaction logic for BlotterView.xaml
    /// </summary>
    public partial class BlotterView : DialogBase, IViewFor<BlotterViewModel>
    {
        #region IViewFor<BlotterViewModel> Members

        public BlotterViewModel ViewModel
        {
            get { return this.DataContext as BlotterViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public BlotterView()
        {
            InitializeComponent();
        }
    }
}
