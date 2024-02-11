using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Common.Person
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class PersonView : UserControl, IViewFor<PersonViewModel>
    {
        #region IViewFor<PersonViewModel> Members

        public PersonViewModel ViewModel
        {
            get { return this.DataContext as PersonViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }
        #endregion

        public PersonView()
        {
            InitializeComponent();
        }
    }
}
