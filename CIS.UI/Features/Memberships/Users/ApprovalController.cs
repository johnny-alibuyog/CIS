﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Memberships;
using CIS.UI.Utilities.Extentions;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CIS.UI.Features.Memberships.Users
{
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
            if (App.Configuration.UserPowerUser == false)
                return;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var powerUser = session.Query<User>().FirstOrDefault(x => x.Username == App.Configuration.PowerUser);
                if (powerUser == null)
                {
                    this.ViewModel.Username = powerUser.Username;
                    this.ViewModel.Password = powerUser.Password;
                }
                transaction.Commit();
            }
        }

        public virtual void Approve()
        {
            var user = (User)null;

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

            if (user.Has(Role.PoliceApprover) == false)
            {
                this.MessageBox.Warn("You are not allowed to compete this action.");
                return;
            }

            this.ViewModel.UserId = user.Id;
            this.ViewModel.Close();
        }
    }
}