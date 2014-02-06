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

namespace CIS.UI.Features.Polices.Warrants.MasterList
{
    /// <summary>
    /// Interaction logic for SuspectView.xaml
    /// </summary>
    public partial class SuspectView : DialogBase, IViewFor<SuspectViewModel>
    {
        #region IViewFor<SuspectViewModel> Members

        public SuspectViewModel ViewModel
        {
            get { return this.DataContext as SuspectViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public SuspectView()
        {
            InitializeComponent();
        }
    }
}
