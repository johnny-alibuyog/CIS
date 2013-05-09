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
using ReactiveUI;

namespace CIS.UI.Features.Polices.Warrants
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl, IViewFor<ListViewModel>
    {
        #region IViewFor<ViewModel>

        private ListViewModel _viewModel;

        public ListViewModel ViewModel 
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        object IViewFor.ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value as ListViewModel; }
        }

        #endregion

        public ListView()
        {
            InitializeComponent();

            DataContext = new ListViewModel();

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
