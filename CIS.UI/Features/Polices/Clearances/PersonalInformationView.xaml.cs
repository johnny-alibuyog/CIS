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
    /// Interaction logic for PersonalInformationView.xaml
    /// </summary>
    public partial class PersonalInformationView : UserControl, IViewFor<PersonalInformationViewModel>
    {
        #region IViewFor<PersonalInformationViewModel> Members

        public PersonalInformationViewModel ViewModel
        {
            get { return this.DataContext as PersonalInformationViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public PersonalInformationView()
        {
            InitializeComponent();
        }
    }
}
