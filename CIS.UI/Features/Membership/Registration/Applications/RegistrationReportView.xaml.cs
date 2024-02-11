using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for RegistrationReportView.xaml
/// </summary>
public partial class RegistrationReportView : DialogBase, IViewFor<RegistrationReportViewModel>
{
    private readonly BindingSource _bindingSource;
    private readonly IContainer _container;

    #region IViewFor<RegistrlationReportViewModel> Members

    public RegistrationReportViewModel ViewModel
    {
        get { return this.DataContext as RegistrationReportViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public RegistrationReportView()
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
        _reportViewer.LocalReport.ReportEmbeddedResource = "CIS.UI.Features.Membership.Registration.Applications.RegistrationReport.rdlc";
        ((ISupportInitialize)(_bindingSource)).EndInit();

        // load report
        this.Loaded += (sender, e) =>
        {
            _bindingSource.DataSource = new object[] { this.ViewModel };
            _reportViewer.RefreshReport();
        };
    }
}
