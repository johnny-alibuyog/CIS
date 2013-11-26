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

namespace CIS.UI.Features.Polices.Clearances
{
    /// <summary>
    /// Interaction logic for ApplicantReportView.xaml
    /// </summary>
    public partial class ApplicantReportView : DialogBase, IViewFor<ApplicantReportViewModel>
    {
        private readonly BindingSource _bindingSource;
        private readonly IContainer _container;

        #region IViewFor<ApplicantReportViewModel> Members

        public ApplicantReportViewModel ViewModel
        {
            get { return this.DataContext as ApplicantReportViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public ApplicantReportView()
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
            _reportViewer.LocalReport.ReportEmbeddedResource = "CIS.UI.Features.Polices.Clearances.ApplicantReport.rdlc";
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
