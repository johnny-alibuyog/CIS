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
    public class LoginController : ControllerBase<LoginViewModel>
    {
        public LoginController(LoginViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Login = new ReactiveCommand(this.ViewModel.IsValidObservable());
            this.ViewModel.Login.Subscribe(x => Login());
            this.ViewModel.Login.ThrownExceptions.Handle(this);

            this.Initialize();
        }

        public virtual void Initialize()
        {
            var powerUser = (User)null;

            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                powerUser = session.Query<User>().FirstOrDefault(x => x.Username == App.Configuration.PowerUser);
                if (powerUser == null)
                {
                    powerUser = new User()
                    {
                        Username = App.Configuration.PowerUser,
                        Password = "admin123",
                        Email = "admin@jlrc.manasoft.com",
                        Person = new Person()
                        {
                            FirstName = "jlrc",
                            MiddleName = "inc",
                            LastName = "manasoft",
                        },
                        Roles = Enum.GetValues(typeof(Role)).Cast<Role>()
                    };

                    session.Save(powerUser);
                }
                transaction.Commit();
            }

            if (App.Configuration.UserPowerUser)
            {
                this.ViewModel.Username = powerUser.Username;
                this.ViewModel.Password = powerUser.Password;
            }
        }

        public virtual void Login()
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

            App.Data.User = user;
            this.ViewModel.Close();
        }
    }
}