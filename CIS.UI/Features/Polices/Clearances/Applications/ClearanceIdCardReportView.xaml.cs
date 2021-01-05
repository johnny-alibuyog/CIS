using Microsoft.Reporting.WinForms;
using ReactiveUI;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CIS.UI.Features.Polices.Clearances.Applications
{
    /// <summary>
    /// Interaction logic for ClearanceIdReportView.xaml
    /// </summary>
    public partial class ClearanceIdCardReportView : DialogBase, IViewFor<ClearanceIdCardReportViewModel>
    {
        private readonly BindingSource _bindingSource;
        private readonly IContainer _container;

        #region IViewFor<ClearanceIdCardReportViewModel> Members

        public ClearanceIdCardReportViewModel ViewModel
        {
            get { return this.DataContext as ClearanceIdCardReportViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ClearanceIdCardReportView()
        {
            InitializeComponent();

            ResizeMode = ResizeMode.CanResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowState = WindowState.Maximized;
            ShowInTaskbar = false;

            // initialize report viewer
            _container = new Container();
            _bindingSource = new BindingSource(_container);

            ((ISupportInitialize)(_bindingSource)).BeginInit();
            _reportViewer.LocalReport.DataSources.Add(new ReportDataSource()
            {
                Name = "ItemDataSet",
                Value = _bindingSource,
            });
            _reportViewer.LocalReport.ReportEmbeddedResource = "CIS.UI.Features.Polices.Clearances.Applications.ClearanceIdCardReport.rdlc";
            ((ISupportInitialize)(_bindingSource)).EndInit();

            // load report
            this.Loaded += (sender, e) =>
            {
                _bindingSource.DataSource = new object[] { this.ViewModel };
                _reportViewer.RefreshReport();
            };

        }

    }
}
