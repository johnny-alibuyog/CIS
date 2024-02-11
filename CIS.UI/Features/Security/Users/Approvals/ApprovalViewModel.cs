using CIS.Core.Domain.Security;
using CIS.UI.Bootstraps.InversionOfControl;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using System;

namespace CIS.UI.Features.Security.Users.Approvals;

public class ApprovalViewModel : ViewModelBase
{
    private readonly ApprovalController _controller;

    public virtual Guid UserId { get; set; }

    [NotNullNotEmpty(Message = "Username is mandatory")]
    public virtual string Username { get; set; }

    [NotNullNotEmpty(Message = "Password is mandatory")]
    public virtual string Password { get; set; }

    public virtual Role[] Roles { get; set; } 

    public virtual IReactiveCommand Approve { get; set; }

    public ApprovalViewModel()
    {
        _controller = IoC.Container.Resolve<ApprovalController>(new ViewModelDependency(this));
    }
}
