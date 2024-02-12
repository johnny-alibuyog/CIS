using System;
using CIS.Core.Utility.Extention;
using CIS.UI.Features.Common.Wizard;

namespace CIS.UI.Features.Membership.Member.Application.Steps;

public class ProfessionalInfoViewModel : WizardStep
{
    public override string Title { get; set; } = typeof(ProfessionalInfoViewModel).Name.SplitToWords();

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

    public static ProfessionalInfoViewModel Create() => new();
}
