﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Bootstraps.InversionOfControl;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Maintenances.Kinds
{
    public class KindListViewModel : ViewModelBase
    {
        private KindListController _controller;

        public virtual string NewItem { get; set; }

        public virtual KindViewModel SelectedItem { get; set; }

        public virtual IReactiveList<KindViewModel> Items { get; set; }

        public virtual IReactiveCommand Load { get; set; }

        public virtual IReactiveCommand Insert { get; set; }

        public virtual IReactiveCommand Delete { get; set; }

        public virtual IReactiveCommand Search { get; set; }

        public KindListViewModel()
        {
            //_controller = new KindListController(this);
            _controller = IoC.Container.Resolve<KindListController>(new ViewModelDependency(this));
        }
    }
}