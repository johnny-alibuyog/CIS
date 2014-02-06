﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Utilities.Extentions;
using ReactiveUI;

namespace CIS.UI.Features.Firearms.Licenses.Registrations
{
    /// <summary>
    /// Interaction logic for LicenseListView.xaml
    /// </summary>
    public partial class LicenseListView : UserControl, IViewFor<LicenseListViewModel>
    {
        #region IViewFor<LicenseListViewModel>

        public LicenseListViewModel ViewModel
        {
            get { return this.DataContext as LicenseListViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion

        public LicenseListView()
        {
            this.InitializeComponent();
            this.InitializeViewModelAsync(() => IoC.Container.Resolve<LicenseListViewModel>());
        }
    }
}