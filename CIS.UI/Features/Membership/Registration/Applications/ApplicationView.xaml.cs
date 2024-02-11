using System.Windows.Controls;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using FirstFloor.ModernUI.Windows;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Registration.Applications;

/// <summary>
/// Interaction logic for ApplicationView.xaml
/// </summary>
public partial class ApplicationView : UserControl, IContent, IViewFor<ApplicationViewModel>
{
    #region IContent Members

    public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
    {
        //this.ViewModel.Reset.Execute(null);
    }

    public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
    {
        //this.ViewModel.Reset.Execute(null);
    }

    public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
    {
        this.ViewModel?.Reset.Execute(null);
    }

    public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
    {
        this.ViewModel?.Reset.Execute(null);
    }

    #endregion

    #region IViewFor<ApplicationViewModel> Members

    public ApplicationViewModel ViewModel
    {
        get { return this.DataContext as ApplicationViewModel; }
        set { this.DataContext = value; }
    }

    object IViewFor.ViewModel
    {
        get { return this.DataContext; }
        set { this.DataContext = value; }
    }

    #endregion

    public ApplicationView()
    {
        //void AddDataTemplate<TViewModel, TView>()
        //    where TViewModel : class
        //    where TView : UserControl, IViewFor<TViewModel>
        //{
        //    var template = new DataTemplate(typeof(TViewModel));
        //    template.VisualTree = new FrameworkElementFactory(typeof(TView));

        //    Resources.Add(typeof(TViewModel), template);
        //}

        //AddDataTemplate<PersonalInformationViewModel, PersonalInformationView>();
        //AddDataTemplate<OtherInformationViewModel, OtherInformationView>();
        //AddDataTemplate<FingerScannerViewModel, FingerScannerView>();
        //AddDataTemplate<CameraViewModel, CameraView>();
        //AddDataTemplate<SignatureViewModel, SignatureView>();
        //AddDataTemplate<FindingViewModel, FindingView>();
        //AddDataTemplate<SummaryViewModel, SummaryView>();

        this.InitializeComponent();
        this.InitializeViewModel(() => IoC.Container.Resolve<ApplicationViewModel>());
    }
}
