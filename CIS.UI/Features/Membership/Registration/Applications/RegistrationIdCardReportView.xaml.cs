using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for RegistrationIdReportView.xaml
/// </summary>
public partial class RegistrationIdCardReportView : DialogBase, IViewFor<RegistrationIdCardReportViewModel>
{
    private readonly BindingSource _bindingSource;
    private readonly IContainer _container;

    #region IViewFor<RegistrationIdCardReportViewModel> Members

    public RegistrationIdCardReportViewModel ViewModel
    {
        get { return this.DataContext as RegistrationIdCardReportViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public RegistrationIdCardReportView()
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
        _reportViewer.LocalReport.ReportEmbeddedResource = "CIS.UI.Features.Membership.Registration.Applications.RegistrationIdCardReport.rdlc";
        ((ISupportInitialize)(_bindingSource)).EndInit();

        // load report
        this.Loaded += (sender, e) =>
        {
            _bindingSource.DataSource = new object[] { this.ViewModel };
            _reportViewer.RefreshReport();
        };

    }

}
