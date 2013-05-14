using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CIS.UI.Utilities.CommonDialogs;
using CIS.UI.Utilities.Extentions;
using ReactiveUI.Xaml;
using Touchless.Vision.Camera;
using Touchless.Vision.Contracts;

namespace CIS.UI.Features.Commons.Cameras
{
    public class CameraController : ControllerBase<CameraViewModel>
    {
        private CameraFrameSource _frameSource;

        public CameraController(CameraViewModel viewModel) : base(viewModel)
        {
            this.ViewModel.Cameras = CameraService.AvailableCameras.ToReactiveColletion();
            this.ViewModel.SelectedCamera = CameraService.DefaultCamera;

            this.ViewModel.Start = new ReactiveCommand();
            this.ViewModel.Start.Subscribe(x => Start());

            this.ViewModel.Stop = new ReactiveCommand();
            this.ViewModel.Stop.Subscribe(x => Stop());

            this.ViewModel.Continue = new ReactiveCommand();
            this.ViewModel.Continue.Subscribe(x => Continue());

            this.ViewModel.Capture = new ReactiveCommand();
            this.ViewModel.Capture.Subscribe(x => Capture());
        }

        private void CaptureNewFrame(IFrameSource frameSource, Frame frame, double fps)
        {
            var bitmap = frame.Image.Clone() as Bitmap; 
            this.ViewModel.ImagePreview = bitmap.ToBitmapSource();
        }

        public virtual void Start()
        {
            this.Stop();

            if (this.ViewModel.SelectedCamera == null)
            {
                MessageDialog.Show("No camera selected or no camera available.", "Police Clearance", MessageBoxButton.OK);
                return;
            }

            var frameSource = new CameraFrameSource(this.ViewModel.SelectedCamera);
            if (_frameSource != frameSource)
                _frameSource = frameSource;

            _frameSource.Camera.CaptureWidth = 320;
            _frameSource.Camera.CaptureHeight = 240;
            _frameSource.Camera.Fps = 20;
            _frameSource.NewFrame += CaptureNewFrame;
            _frameSource.StartFrameCapture();
        
        }

        public virtual void Stop()
        {
            // Trash the old camera
            if (_frameSource != null)
            {
                _frameSource.StopFrameCapture();
                _frameSource.NewFrame -= CaptureNewFrame;
                _frameSource.Camera.Dispose();
            }
        }

        public virtual void Continue()
        {
            this.Stop();
            this.Start();
        }

        public virtual void Capture()
        {
            this.ViewModel.Picture = this.ViewModel.ImagePreview;
        }
    }
}
