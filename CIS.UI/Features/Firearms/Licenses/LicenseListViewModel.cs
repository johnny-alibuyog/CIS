﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class LicenseListViewModel : ViewModelBase
    {
        private readonly LicenseListController _controller;

        public virtual LicenseListCriteriaViewModel Criteria { get; set; }

        public virtual LicenseListItemViewModel SelectedItem { get; set; }

        public virtual ReactiveCollection<LicenseListItemViewModel> Items { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public virtual IReactiveCommand Create { get; set; }

        public virtual IReactiveCommand Edit { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public LicenseListViewModel()
        {
            Criteria = new LicenseListCriteriaViewModel();

            _controller = new LicenseListController(this);
        }
    }
}
