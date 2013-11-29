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

namespace CIS.UI.Features.Commons.Persons
{
    /// <summary>
    /// Interaction logic for BasicPersonView.xaml
    /// </summary>
    public partial class PersonBasicView : UserControl, IViewFor<PersonBasicViewModel>
    {
        public PersonBasicView()
        {
            InitializeComponent();
        }

        #region IViewFor<BasicPersonViewModel> Members

        public PersonBasicViewModel ViewModel
        {
            get { return this.DataContext as PersonBasicViewModel; }
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
