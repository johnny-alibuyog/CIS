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
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;
using ReactiveUI;

namespace CIS.UI.Features.Barangays.Clearances.Archives
{
    /// <summary>
    /// Interaction logic for ArchiveReportView.xaml
    /// </summary>
    public partial class ArchiveReportView : DialogBase, IViewFor<ArchiveReportViewModel>
    {
        private readonly BindingSource _bindingSource;
        private readonly IContainer _container;

        #region IViewFor<ArchiveReportViewModel> Members

        public ArchiveReportViewModel ViewModel
        {
            get { return this.DataContext as ArchiveReportViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ArchiveReportView()
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
            _reportViewer.LocalReport.ReportEmbeddedResource = "CIS.UI.Features.Barangays.Clearances.Archives.ArchiveReport.rdlc";
            ((ISupportInitialize)(_bindingSource)).EndInit();

            // load report
            this.Loaded += (sender, e) =>
            {
                _bindingSource.DataSource = this.ViewModel.Items;
                _reportViewer.LocalReport.SetParameters(new ReportParameter("Office", this.ViewModel.Office));
                _reportViewer.LocalReport.SetParameters(new ReportParameter("Location", this.ViewModel.Location));
                _reportViewer.LocalReport.SetParameters(new ReportParameter("IncludeDate", this.ViewModel.FilterDate.ToString()));
                _reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", this.ViewModel.FromDate.ToString()));
                _reportViewer.LocalReport.SetParameters(new ReportParameter("ToDate", this.ViewModel.ToDate.ToString()));
                _reportViewer.RefreshReport();
            };
        }
    }
}
