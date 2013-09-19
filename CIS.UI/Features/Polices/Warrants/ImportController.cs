using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.Core.Entities.Commons;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Polices.Warrants
{
    public class ImportController : ControllerBase<ImportViewModel>
    {
        private readonly BackgroundWorker _importWorker;

        public ImportController(ImportViewModel viewModel)
            : base(viewModel)
        {
            _importWorker = new BackgroundWorker();
            _importWorker.DoWork += (sender, e) => Import();
            //_backgroundWorker.ProgressChanged += (sender, e) => ProgressChanged(this.ViewModel));
            //_backgroundWorker.RunWorkerCompleted += (sender, e) => RunWorkerCompleted(this.ViewModel));
            _importWorker.WorkerReportsProgress = true;
            _importWorker.WorkerSupportsCancellation = true;

            this.ViewModel.LookupPath = new ReactiveCommand();
            this.ViewModel.LookupPath.Subscribe(x => LookupPath());

            this.ViewModel.Reset = new ReactiveCommand();
            this.ViewModel.Reset.Subscribe(x => Reset());

            this.ViewModel.Import = new ReactiveCommand();
            this.ViewModel.Import.Subscribe(x => RunImportWorker());
        }

        [HandleError]
        public virtual void LookupPath()
        {
            var openDirectoryDialog = IoC.Container.Resolve<IOpenDirectoryDialogService>();
            var result = openDirectoryDialog.Show();
            if (result != null)
                this.ViewModel.SourcePath = result;
        }

        [HandleError]
        public virtual void Reset()
        {
            var confirmed = this.MessageBox.Confirm("Do you want to reset?", "Import");
            if (confirmed == false)
                return;

            this.ViewModel.SourcePath = string.Empty;
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalCases = null;
            this.ViewModel.TotalSuspects = null;
            this.ViewModel.Status = string.Empty;
        }

        [HandleError]
        public virtual void RunImportWorker()
        {
            if (!Directory.Exists(this.ViewModel.SourcePath))
            {
                this.MessageBox.Warn("Please specify valid directory", "Import");
                return;
            }

            var confirmed = this.MessageBox.Confirm("Do you want to import from this directory?", "Import");
            if (confirmed == false)
                return;

            _importWorker.RunWorkerAsync();
        }

        [HandleError]
        public virtual void Import()
        {
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalCases = 0M;
            this.ViewModel.TotalSuspects = 0M;
            var start = DateTime.Now;

            var importer = (IImportService)null;

            importer = (IImportService)IoC.Container.Resolve<LitusImportService>(new Dependency("viewModel", this.ViewModel));
            importer.Execute();

            importer = (IImportService)IoC.Container.Resolve<NaraImportService>(new Dependency("viewModel", this.ViewModel));
            importer.Execute();

            var end = DateTime.Now;
            var duration = end - start;

            this.ViewModel.ImportStart = start;
            this.ViewModel.ImportEnd = end;
            this.ViewModel.TotalTime = duration;
        }
    }
}
