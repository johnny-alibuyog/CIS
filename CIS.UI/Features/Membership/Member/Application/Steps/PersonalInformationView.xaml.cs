using System.Windows.Controls;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;
/// <summary>
/// Interaction logic for PersonalInformationView.xaml
/// </summary>
public partial class PersonalInformationView : UserControl, IViewFor<PersonalInformationViewModel>
{
    public PersonalInformationViewModel ViewModel
    {
        get => this.DataContext as PersonalInformationViewModel;
        set => this.DataContext = value;
    }

    object IViewFor.ViewModel
    {
        get => this.DataContext;
        set => this.DataContext = value;
    }

    public PersonalInformationView()
    {
        InitializeComponent();

        //this.Bind(ViewModel, x => x.Person.Prefix, x => x.Prefix.Text);
        //this.Bind(ViewModel, x => x.Person.FirstName, x => x.FirstName.Text);
        //this.Bind(ViewModel, x => x.Person.MiddleName, x => x.MiddleName.Text);
        //this.Bind(ViewModel, x => x.Person.LastName, x => x.LastName.Text);
        //this.Bind(ViewModel, x => x.Person.Suffix, x => x.Suffix.Text);
        //this.Bind(ViewModel, x => x.Person.BirthDate, x => x.BirthDate.Text);
        //this.Bind(ViewModel, x => x.Person.BirthPlace, x => x.BirthPlace.Text);
        //this.Bind(ViewModel, x => x.St, x => x.BirthPlace.Text);

    }
}
