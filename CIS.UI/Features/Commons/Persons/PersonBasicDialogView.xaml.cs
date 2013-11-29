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
using CIS.UI.Utilities.Extentions;
using CIS.UI.Bootstraps.InversionOfControl;

namespace CIS.UI.Features.Commons.Persons
{
    /// <summary>
    /// Interaction logic for BasicPersonDialogView.xaml
    /// </summary>
    public partial class PersonBasicDialogView : DialogBase, IViewFor<PersonBasicDialogViewModel>
    {
        public PersonBasicDialogView()
        {
            this.InitializeComponent();
            //this.InitializeViewModelAsync(() => IoC.Container.Resolve<BasicPersonDialogViewModel>());
        }

        #region IViewFor<BasicPersonDialogViewModel> Members

        public PersonBasicDialogViewModel ViewModel
        {
            get { return this.DataContext as PersonBasicDialogViewModel; }
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
