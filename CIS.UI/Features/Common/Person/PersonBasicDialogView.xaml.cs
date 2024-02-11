using ReactiveUI;

namespace CIS.UI.Features.Common.Person;

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
