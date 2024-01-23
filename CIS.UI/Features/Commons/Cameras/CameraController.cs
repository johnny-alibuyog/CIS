using CIS.UI.Utilities.Extentions;
using ReactiveUI;
using System;
using System.Drawing;
using System.IO;
using Touchless.Vision.Camera;
using Touchless.Vision.Contracts;

namespace CIS.UI.Features.Commons.Cameras;

public class CameraController : ControllerBase<CameraViewModel>
{
    private CameraFrameSource _frameSource;

    public CameraController(CameraViewModel viewModel)
        : base(viewModel)
    {
        this.ViewModel.Cameras = CameraService.AvailableCameras.ToReactiveList();
        this.ViewModel.SelectedCamera = CameraService.DefaultCamera;

        this.ViewModel.Start = new ReactiveCommand();
        this.ViewModel.Start.Subscribe(x => Start());
        this.ViewModel.Start.ThrownExceptions.Handle(this);

        this.ViewModel.Stop = new ReactiveCommand();
        this.ViewModel.Stop.Subscribe(x => Stop());
        this.ViewModel.Stop.ThrownExceptions.Handle(this);

        this.ViewModel.Continue = new ReactiveCommand();
        this.ViewModel.Continue.Subscribe(x => Continue());
        this.ViewModel.Continue.ThrownExceptions.Handle(this);

        this.ViewModel.Capture = new ReactiveCommand();
        this.ViewModel.Capture.Subscribe(x => Capture());
        this.ViewModel.Capture.ThrownExceptions.Handle(this);
    }

    private void CaptureNewFrame(IFrameSource frameSource, Frame frame, double fps)
    {
        var bitmap = frame.Image.Clone() as Bitmap;
        //this.ViewModel.ImagePreview = bitmap.ToBitmapSource();

#if DEBUG
        bitmap.Save(Path.Combine(App.Config.ApplicationDataLocation, "rawPicture.bmp"));
        bitmap.ReduceSize(App.Data.Image.PictureResizeScaleFactor).Save(Path.Combine(App.Config.ApplicationDataLocation, "reducedPicture.bmp"));
#endif

        this.ViewModel.ImagePreview = bitmap.ReduceSize(App.Data.Image.PictureResizeScaleFactor).ToBitmapSource();
    }

    public virtual void Start()
    {
        this.Stop();

        if (this.ViewModel.SelectedCamera == null)
        {
            this.MessageBox.Warn("No camera selected or no camera available.", "Police Clearance");
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
        // dispose the old camera
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
        //this.ViewModel.Picture = this.ViewModel.ImagePreview.ReduceSize(App.Config.PictureResizeScaleFactor);

    }
}
