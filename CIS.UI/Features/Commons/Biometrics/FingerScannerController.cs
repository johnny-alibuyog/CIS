using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Polices;
using CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors;
using CIS.UI.Utilities.Extentions;
using DPFP;
using DPFP.Capture;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace CIS.UI.Features.Commons.Biometrics
{
    [HandleError]
    public class FingerScannerController : ControllerBase<FingerScannerViewModel>, DPFP.Capture.EventHandler
    {
        private readonly Capture _scanner;
        private IList<FingerViewModel> _fingersToScan;

        public FingerScannerController(FingerScannerViewModel viewModel)
            : base(viewModel)
        {
            InitializeFingersToScan();

            _scanner = new Capture();
            _scanner.EventHandler = this;

            this.ViewModel.Stop = new ReactiveCommand();
            this.ViewModel.Stop.Subscribe(x => Stop());
            this.ViewModel.Stop.ThrownExceptions.Handle(this);

            this.ViewModel.Start = new ReactiveCommand();
            this.ViewModel.Start.Subscribe(x => Start());
            this.ViewModel.Start.ThrownExceptions.Handle(this);
        }

        #region Routine Helpers

        private void InitializeFingersToScan()
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var fingerIds = session.Query<Setting>()
                    .Where(x => x.Terminal.MachineName == Environment.MachineName)
                    .SelectMany(x => x.FingersToScan)
                    .Select(x => x.Id)
                    .Cacheable()
                    .ToList();

                _fingersToScan = FingerViewModel.GetByIds(fingerIds);

                transaction.Commit();
            }
        }

        private FingerViewModel GetNextFingerToScan()
        {
            var current = this.ViewModel.CurrentFinger;
            if (_fingersToScan.LastOrDefault() == current)
                return _fingersToScan.FirstOrDefault();
            else
                return _fingersToScan[_fingersToScan.IndexOf(current) + 1];
        }

        private void Process(Sample sample)
        {
            var image = ConvertSampleToBitmap(sample).ToBitmapSource();

            this.ViewModel.CapturedFingerImage = image;
            this.ViewModel.FingerImages[this.ViewModel.CurrentFinger] = image;
            this.ViewModel.CurrentFinger = GetNextFingerToScan();
        }

        private Bitmap ConvertSampleToBitmap(Sample sample)
        {
            var converter = new SampleConversion();
            var bitmap = (Bitmap)null;
            converter.ConvertToPicture(sample, ref bitmap);

#if DEBUG
            bitmap.Save(Path.Combine(App.Config.ApplicationDataLocation, "rawFingerPrint.bmp"));
            bitmap.ReduceSize().Save(Path.Combine(App.Config.ApplicationDataLocation, "reducedFingerPrint.bmp"));
#endif

            //return bitmap;

            return bitmap.ReduceSize(App.Data.Image.FingerPrintResizeScaleFactor);
        }

        #endregion

        #region Public Members

        public virtual void Start()
        {
            if (_scanner != null)
            {
                try
                {
                    _scanner.StartCapture();
                    this.ViewModel.EventLogs.Add("Using the fingerprint reader, scan your fingerprint.");
                }
                catch
                {
                    this.ViewModel.EventLogs.Add("Can't initiate capture!");
                }
            }
        }

        public virtual void Stop()
        {
            if (_scanner != null)
            {
                try
                {
                    _scanner.StopCapture();
                }
                catch
                {
                    this.ViewModel.Prompt = "Can't terminate capture!";
                }
            }
        }

        #endregion

        #region DPFP.Capture.EventHandler Members

        public virtual void OnComplete(object captured, string readerSerialNumber, Sample sample)
        {
            DispatcherInvoke(() =>
            {
                this.ViewModel.EventLogs.Add("The fingerprint sample was captured.");
                this.ViewModel.Prompt = "The fingerprint was succesfuly captured.";
                Process(sample);
            });
        }

        public virtual void OnFingerGone(object captured, string readerSerialNumber)
        {
            DispatcherInvoke(() => this.ViewModel.EventLogs.Insert(0, "The finger was removed from the fingerprint reader."));
        }

        public virtual void OnFingerTouch(object capture, string readerSerialNumber)
        {
            DispatcherInvoke(() => this.ViewModel.EventLogs.Insert(0, "The fingerprint reader was touched."));
        }

        public virtual void OnReaderConnect(object capture, string readerSerialNumber)
        {
            DispatcherInvoke(() => this.ViewModel.EventLogs.Insert(0, "The fingerprint reader was connected."));
        }

        public virtual void OnReaderDisconnect(object capture, string readerSerialNumber)
        {
            DispatcherInvoke(() => this.ViewModel.EventLogs.Insert(0, "The fingerprint reader was disconnected."));
        }

        public virtual void OnSampleQuality(object capture, string readerSerialNumber, CaptureFeedback captureFeedback)
        {
            DispatcherInvoke(() =>
            {
                if (captureFeedback == CaptureFeedback.Good)
                    this.ViewModel.EventLogs.Insert(0, "The quality of the fingerprint sample is good.");
                else
                    this.ViewModel.EventLogs.Insert(0, "The quality of the fingerprint sample is poor.");
            });
        }

        #endregion
    }
}
