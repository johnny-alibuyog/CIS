﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Memberships;
using CIS.UI.Utilities.Extentions;

namespace CIS.UI.Features.Memberships.Users
{
    public class UserRoleViewModel : ViewModelBase
    {
        public virtual bool IsChecked { get; set; }

        public virtual Role Role { get; set; }

        public virtual string Description { get { return this.Role.GetDescription(); } }

        public UserRoleViewModel() { }

        public UserRoleViewModel(bool isChecked, Role role)
        {
            this.IsChecked = isChecked;
            this.Role = role;
        }


    }
}