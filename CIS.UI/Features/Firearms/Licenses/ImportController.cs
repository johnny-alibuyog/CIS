using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using CIS.UI.Bootstraps.DependencyInjection;
using CIS.UI.Utilities.CommonDialogs;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Firearms.Licenses
{
    public class ImportController : ControllerBase<ImportViewModel>
    {
        private readonly BackgroundWorker _importWorker;

        public ImportController(ImportViewModel viewModel) : base(viewModel)
        {
            _importWorker = new System.ComponentModel.BackgroundWorker();
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

        public virtual void LookupPath()
        {
            var result = OpenDirectoryDialog.Show();
            if (result != null)
                this.ViewModel.SourcePath = result;
        }

        public virtual void Reset()
        {
            var confirm = MessageDialog.Show("Do you want to reset?", "Import", MessageBoxButton.OK);
            if (confirm == false)
                return;

            this.ViewModel.SourcePath = string.Empty;
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalTime = null;
            this.ViewModel.TotalLicenses = null;
            this.ViewModel.Status = string.Empty;
        }

        public virtual void RunImportWorker()
        {
            if (!Directory.Exists(this.ViewModel.SourcePath))
            {
                MessageDialog.Show("Please specify valid directory", "Import", MessageBoxButton.OK);
                return;
            }

            var confirm = MessageDialog.Show("Do you want to import from this directory?", "Import", MessageBoxButton.YesNo);
            if (confirm == false)
                return;

            _importWorker.RunWorkerAsync();
        }

        public virtual void Import()
        {
            this.ViewModel.ImportStart = null;
            this.ViewModel.ImportEnd = null;
            this.ViewModel.TotalLicenses = 0M;

            var start = DateTime.Now;

            var importer = IoC.Container.Resolve<ImportDataInitializer>();
            importer.ViewModel = this.ViewModel;
            importer.Execute();

            var end = DateTime.Now;
            var duration = end - start;

            this.ViewModel.ImportStart = start;
            this.ViewModel.ImportEnd = end;
            this.ViewModel.TotalTime = duration;
        }
    }
}
