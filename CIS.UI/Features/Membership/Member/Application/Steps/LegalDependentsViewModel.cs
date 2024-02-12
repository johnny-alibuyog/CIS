using System;
using System.Collections.Generic;
using System.Linq;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using CIS.UI.Features.Common.Wizard;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;

public class LegalDependentsViewModel : WizardStep
{
    public override string Title { get; set; } = typeof(LegalDependentsViewModel).Name.SplitToWords();

    public LegalDependentLookup Lookup { get; set; } = new LegalDependentLookup();

    public LegalDependentsModel Data { get; set; } = new LegalDependentsModel();

    public IReactiveCommand NewDependentCommand { get; set; }

    public IReactiveCommand RemoveDependentCommand { get; set; }

    public LegalDependentsViewModel()
    {
        NewDependentCommand = new ReactiveCommand();
        NewDependentCommand.Subscribe(_ => NewDependent());
        NewDependentCommand.ThrownExceptions.Handle(this);

        RemoveDependentCommand = new ReactiveCommand();
        RemoveDependentCommand.Subscribe(value => RemoveDependent(value as DependentModel));
        RemoveDependentCommand.ThrownExceptions.Handle(this);
    }

    private void NewDependent()
    {
        Data.Dependents.Add(DependentModel.Empty);
    }

    private void RemoveDependent(DependentModel dependent)
    {
        if (dependent is null)
            return;

        Data.Dependents.Remove(dependent);
    }

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

    public static LegalDependentsViewModel Create() => new();
}

public class LegalDependentLookup()
{
    public IEnumerable<Relationship> Relationships { get; set; } = Enum.GetValues(typeof(Relationship)).Cast<Relationship>();
}

public class DependentModel(Relationship relationship, string name, DateTime? birthDate)
{
    public Relationship Relationship { get; set; } = relationship;
    public string Name { get; set; } = name;
    public DateTime? BirthDate { get; set; } = birthDate;

    public static DependentModel Empty => new(Relationship.Child, string.Empty, null);
}

public class LegalDependentsModel
{
    public IReactiveList<DependentModel> Dependents { get; set; } = new ReactiveList<DependentModel>()
    {
        new(Relationship.Spouse, "John Doe", new DateTime(1980, 1, 1)),
        new(Relationship.Child, "Jane Doe", new DateTime(2000, 1, 1)),
        new(Relationship.Child, "Jack Doe", new DateTime(2005, 1, 1)),
        new(Relationship.Child, "Jill Doe", new DateTime(2010, 1, 1))
    };
}
