using System;
using System.Collections.Generic;
using CIS.Core.Domain.Common;
using CIS.Core.Utility.Extention;
using CIS.UI.Features.Common.Wizard;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Membership.Member.Application.Steps;

public class EducationalAttainmentViewModel : WizardStep
{
    public override string Title { get; set; } = typeof(EducationalAttainmentViewModel).Name.SplitToWords();

    public EducationalAttainmentModel Data { get; set; } = new();

    public string NewSkill { get; set; }

    public string NewHobby { get; set; }

    public IReactiveCommand AddSkillCommand { get; set; }

    public IReactiveCommand RemoveSkillCommand { get; set; }

    public IReactiveCommand AddHobbyCommand { get; set; }

    public IReactiveCommand RemoveHobbyCommand { get; set; }

    private IObservable<bool> CanAddSkill => this.WhenAnyValue(x => x.NewSkill, x => !string.IsNullOrWhiteSpace(x));

    private IObservable<bool> CanAddHobby => this.WhenAnyValue(x => x.NewHobby, x => !string.IsNullOrWhiteSpace(x));

    public EducationalAttainmentViewModel()
    {
        AddSkillCommand = new ReactiveCommand(CanAddSkill);
        AddSkillCommand.Subscribe(_ => AddSkill());
        AddSkillCommand.ThrownExceptions.Handle(this);

        RemoveSkillCommand = new ReactiveCommand();
        RemoveSkillCommand.Subscribe(value => RemoveSkill(value as string));
        RemoveSkillCommand.ThrownExceptions.Handle(this);

        AddHobbyCommand = new ReactiveCommand(CanAddHobby);
        AddHobbyCommand.Subscribe(_ => AddHobby());
        AddHobbyCommand.ThrownExceptions.Handle(this);

        RemoveHobbyCommand = new ReactiveCommand();
        RemoveHobbyCommand.Subscribe(value => RemoveHobby(value as string));
        RemoveHobbyCommand.ThrownExceptions.Handle(this);
    }

    private void AddSkill()
    {
        Data.Skills.Add(NewSkill);
        NewSkill = string.Empty;
    }

    private void RemoveSkill(string value)
    {
        if (value is null)
            return;
        
        Data.Skills.Remove(value);
    }

    private void AddHobby()
    {
        Data.Hobbies.Add(NewHobby);
        NewHobby = string.Empty;
    }

    private void RemoveHobby(string value)
    {
        if (value is null)
            return;

        Data.Hobbies.Remove(value);
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

    public static EducationalAttainmentViewModel Create() => new();
}

public class EducationModel(EducationLevel educationLevel, string schoolName, DateTime? dateGraduated) : ReactiveObject
{
    public EducationLevel Level { get; set; } = educationLevel;
    public string SchoolName { get; set; } = schoolName;
    public DateTime? DateGraduated { get; set; } = dateGraduated;

    public static IEnumerable<EducationModel> List =>
    [
        new(EducationLevel.Elementary, null, null),
        new(EducationLevel.HighSchool, null, null),
        new(EducationLevel.College, null, null),
        new(EducationLevel.Vocational, null, null),
        new(EducationLevel.Masters, null, null),
        new(EducationLevel.Doctorate, null, null)
    ];
}

public class EducationalAttainmentModel : ReactiveObject
{
    public IReactiveList<EducationModel> Educations { get; set; } = EducationModel.List.ToReactiveList();
    public IReactiveList<string> Skills { get; set; } = new[] { "Skill1", "Skill2" }.ToReactiveList();
    public IReactiveList<string> Hobbies { get; set; } = new[] { "Hobby1", "Hobby2" }.ToReactiveList();
}
