//*****************************************************************************************
//  File:       TouchlessMgr.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//              John Conwell
//
//  Defines a class representing a camera
//*****************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using WebCamLib;

namespace TouchlessLib
{
    /// <summary>
    /// Represents a camera in use by the Touchless system
    /// </summary>
    public class Camera : IDisposable
    {
        #region Public Interface

        /// <summary>
        /// Gets the DirectShow device index of the camera
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Defines the frames per second limit that is in place, -1 means no limit
        /// </summary>
        public int FpsLimit
        {
            get { return _fpslimit; }
            set
            {
                _fpslimit = (value <= 0)? -1 : value;
                _framesSent = 0;
                _cameraTimer.Reset();
                _cameraTimer.Start();
            }
        }

        /// <summary>
        /// Defines the width of the image captured
        /// </summary>
        public int CaptureWidth { get; private set; }

        /// <summary>
        /// Defines the height of the image captured
        /// </summary>
        public int CaptureHeight { get; private set; }

        /// <summary>
        /// Command for rotating and flipping incoming images
        /// </summary>
        public RotateFlipType RotateFlip
        {
            get { return _rotateFlip; }
            set
            {
                // Swap height/width when rotating by 90 or 270
                if ((int)_rotateFlip % 2 != (int)value % 2)
                {
                    int temp = CaptureWidth;
                    CaptureWidth = CaptureHeight;
                    CaptureHeight = temp;
                }
                _rotateFlip = value;
            }
        }

        /// <summary>
        /// Shows a camera specific properties dialog
        /// </summary>
        /// <param name="handle">Native window handle of the dialog parent</param>
        public void ShowPropertiesDialog(IntPtr handle)
        {
            Camera._CameraMethods.DisplayCameraPropertiesDialog(Index, handle);
        }

        /// <summary>
        /// Returns the last image acquired from the camera
        /// </summary>
        /// <returns>A bitmap of the last image acquired from the camera</returns>
        /// <example>
        /// The following is a code snipet that shows acquiring the the current image from the camera. 
        /// Selecting a camera is omitted from this sample.
        /// <code lang="cs">
        ///     TouchlessMgr touch = new TouchlessMgr();
        /// 
        ///     // Code to select the camera omitted
        ///     ...
        /// 
        ///     Bitmap b = touch.CurrentCamera.GetCurrentImage();
        /// </code>
        /// </example>
        public Bitmap GetCurrentImage()
        {
            if (_bitmap == null)
                return null;
            lock (_bitmap) { return (Bitmap)_bitmap.Clone(); }
        }

        /// <summary>
        /// Event fired when an image from the camera is captured
        /// </summary>
        /// <example>
        /// The following is a code snippet that shows how to attach to the <c>OnImageCaptured</c> event to process images captured by the current camera.
        /// <code lang="cs">
        ///     TouchlessMgr touch = new TouchlessMgr();
        /// 
        ///     // Code to select the camera omitted
        ///     ...
        /// 
        ///     touch.CurrentCamera.OnImageCaptured += new EventHandler&lt;CameraEventArgs&gt;(Camera_OnImageCaptured);
        ///     
        ///     ...
        /// 
        ///     void Camera_OnImageCaptured(object sender, CameraEventArgs args)
        ///     {
        ///         Camera c = (Camera)sender;
        /// 
        ///         // Do something with args.Image
        ///     }
        /// </code>
        /// </example>
        public event EventHandler<CameraEventArgs> OnImageCaptured;

        /// <summary>
        /// Returns the camera name as the ToString implementation
        /// </summary>
        /// <returns>The name of the camera</returns>
        public override string ToString() { return _name; }

        #endregion Public Interface

        #region Internal Implementation

        /// <summary>
        /// Camera onstructor
        /// </summary>
        /// <param name="index">The unique index of the camera</param>
        /// <param name="name">The name of the camera</param>
        internal Camera(int index, string name)
        {
            Index = index;
            _name = name;
            CaptureWidth = 320;
            CaptureHeight = 240;
            _cameraTimer.Start();
            _bitmap = new Bitmap(1, 1);
        }

        /// <summary>
        /// DirectShow interface callback
        /// </summary>
        /// <param name="bitmap">The captured bitmap</param>
        internal void ImageCaptured(Bitmap bitmap)
        {
            if (OnImageCaptured == null || _cameraTimer.ElapsedMilliseconds == 0)
                return;

            // Only invoke the callback if the fps limit isn't satisfied
            float cameraFps = (1000f * _framesSent) / _cameraTimer.ElapsedMilliseconds;
            if (_fpslimit == -1 || cameraFps <= _fpslimit)
            {
                _framesSent++;
                // Save the bitmap
                lock (_bitmap) { _bitmap = bitmap; }
                OnImageCaptured.Invoke(this, new CameraEventArgs(bitmap, cameraFps));

                // Reset FPS calc every 5 seconds
                if (_cameraTimer.ElapsedMilliseconds >= 5000)
                {
                    _framesSent = 0;
                    _cameraTimer.Reset();
                    _cameraTimer.Start();
                }
            }
        }

        // Camera Methods - DirectShow interface wrapper
        internal static CameraMethods _CameraMethods = null;

        readonly private string _name;
        private Bitmap _bitmap;
        private int _fpslimit = -1;
        private uint _framesSent = 0;
        private RotateFlipType _rotateFlip = RotateFlipType.RotateNoneFlipNone;
        readonly private Stopwatch _cameraTimer = new Stopwatch();
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleanup function for the camera
        /// </summary>
        public void Dispose() {}

        #endregion
    }

    /// <summary>
    /// Camera specific EventArgs that provides the Image being captured
    /// </summary>
    public class CameraEventArgs : EventArgs
    {
        /// <summary>
        /// Current Camera Image
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// The actual frames per second delivered to the OnImageCaptured callback
        /// </summary>
        public float CameraFps { get; private set; }

        /// <summary>
        /// CameraEventArgs constructor
        /// </summary>
        /// <param name="image">The bitmap to post</param>
        /// <param name="cameraFps">The camera fps to post</param>
        internal CameraEventArgs(Bitmap image, float cameraFps) { Image = image; CameraFps = cameraFps; }
    }
}
