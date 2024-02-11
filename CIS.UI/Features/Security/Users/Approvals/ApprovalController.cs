using CIS.Core.Domain.Security;
using CIS.UI.Utilities.Extentions;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Linq;

namespace CIS.UI.Features.Security.Users.Approvals;

public class ApprovalController : ControllerBase<ApprovalViewModel>
{
    public ApprovalController(ApprovalViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Approve = new ReactiveCommand(this.ViewModel.IsValidObservable());
        this.ViewModel.Approve.Subscribe(x => Approve());
        this.ViewModel.Approve.ThrownExceptions.Handle(this);

        this.Initialize();
    }

    public virtual void Initialize()
    {
        if (App.Config.Login.UsePowerUser == false)
            return;

        using var session = this.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var powerUser = session.Query<User>().FirstOrDefault(x => x.Username == App.Config.Login.PowerUser);
        if (powerUser == null)
        {
            this.ViewModel.Username = powerUser.Username;
            this.ViewModel.Password = powerUser.Password;
        }
        transaction.Commit();
    }

    public virtual void Approve()
    {
        var user = default(User);

        using (var session = this.SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            var userQuery = session.Query<User>()
                .Where(x =>
                    x.Username == this.ViewModel.Username &&
                    x.Password == this.ViewModel.Password
                )
                .Fetch(x => x.Roles)
                .ToFutureValue();

            user = userQuery.Value;

            transaction.Commit();
        }

        if (user == null)
        {
            this.MessageBox.Warn("Invalid username or password.");
            return;
        }

        if (user.Has(this.ViewModel?.Roles ?? []) == false)
        {
            this.MessageBox.Warn("You are not allowed to compete this action.");
            return;
        }

        this.ViewModel.UserId = user.Id;
        this.ViewModel.Close();
    }
}
