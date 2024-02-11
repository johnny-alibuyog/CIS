using System;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using CIS.UI.Features.Common.Person;
using CIS.UI.Features.Common.Wizard;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;

public class PersonalInformationViewModel : WizardStep
{
    public override string Title { get; set; } = typeof(PersonalInformationViewModel).Name.SplitToWords();

    public PersonViewModel Person { get; set; } = new PersonViewModel();

    public virtual CivilStatus? CivilStatus { get; set; }

    public virtual IReactiveList<CivilStatus> CivilStatuses { get; set; } = Enum.GetValues(typeof(CivilStatus)).Cast<CivilStatus>().ToReactiveList();

    public override void Reset()
    {
        Console.WriteLine($"{this.Title} is resetting ...");
    }

    public override void Load()
    {
        Console.WriteLine($"{this.Title} is loading ...");
    }

    public override void Unload()
    {
        Console.WriteLine($"{this.Title} is unloading ...");
    }

    public static PersonalInformationViewModel Create()
    {
        return new PersonalInformationViewModel();
    }
}
