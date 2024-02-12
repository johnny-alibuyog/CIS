using System;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Features.Membership.Member.Application.Steps;

namespace CIS.UI.Features.Membership.Member.Application;

[HandleError]
public class ApplicationController : ControllerBase<ApplicationViewModel>
{
    public ApplicationController(ApplicationViewModel viewModel) : base(viewModel)
    {
        this.ViewModel.Wizard = new(
            steps: [
                PersonalInfoViewModel.Create(),
                ProfessionalInfoViewModel.Create(),
                EducationalAttainmentViewModel.Create(),
                LegalDependentsViewModel.Create(),
                MembershipInfoViewModel.Create(),
            ],
            onReset: () => Console.WriteLine("Reset"),
            onSubmit: () => Console.WriteLine("Submit")
        );
    }
}
