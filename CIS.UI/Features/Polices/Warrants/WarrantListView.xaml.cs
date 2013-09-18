using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Polices.Warrants
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl, IViewFor<WarrantListViewModel>
    {
        #region IViewFor<ViewModel>

        public WarrantListViewModel ViewModel 
        {
            get { return this.DataContext as WarrantListViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ListView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<WarrantListViewModel>());

            //this.ViewModel = new WarrantListViewModel();

            //if (!DesignerProperties.GetIsInDesignMode(this))
            //{
            //    this.ViewModel = new ViewModel();

            //    this.Bind(this.ViewModel, x => x.Criteria.FirstName, x => x.FirstNameTextBox.Text);
            //    this.Bind(this.ViewModel, x => x.Criteria.MiddleName, x => x.MiddleNameTextBox.Text);
            //    this.Bind(this.ViewModel, x => x.Criteria.LastName, x => x.LastNameTextBox.Text);
            //    this.Bind(this.ViewModel, x => x.Items, x => x.Items);
            //}
        }
    }
}
