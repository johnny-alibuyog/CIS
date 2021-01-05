//*****************************************************************************************
//  File:       TouchlessMgr.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//              John Conwell
//
//  TODO: Implement additional marker data (ColorAvg, ColorSpace, Axis, Roundness...)
//  TODO: Add flood fill alrogithm to GetMarkerAppearance?
//  TODO: Add RefineMarkerAppearance? (provide subsequent examples of a marker appearance)
//  TODO: Periodically/continuously adopt surrounding pixels of confirmed marker pixels...
//  TODO: Single function for RGB->HSV->Binned->Hash#... (reduce loop overhead)
//  TODO: Fix the search bounds hack for smoothing enabled (store raw&smoothed values?)
//  TODO: Smarter updating of command lists between consecutive images / row sections?
//  TODO: Improve the search bounds of a previously absent marker. (or scan sparsely)
//  TODO: Add Markers publicly accessible member...
//  TODO: New marker add method: use n>1 consecutive images, detect motion... (maybe...)
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WebCamLib;

namespace TouchlessLib
{
    /// <summary>
    /// Main class for the Touchless library.  Allows access to cameras on the
    /// system and manipulation of Markers
    /// </summary>
    public class TouchlessMgr : IDisposable
    {
        #region Public Interface

        /// <summary>
        /// Refreshes the list of available cameras.  All existing camera instances are invalidated
        /// </summary>
        public void RefreshCameraList()
        {
            if (Camera._CameraMethods != null)
                Camera._CameraMethods.Dispose();
            Camera._CameraMethods = new CameraMethods();

            _cameras = new List<Camera>();
            int count = Camera._CameraMethods.Count;
            for (int n = 0; n < count; n++)
            {
                CameraInfo camInfo = Camera._CameraMethods.GetCameraInfo(n);
                Camera cam = new Camera(camInfo.index, camInfo.name);
                _cameras.Add(cam);
            }
        }

        /// <summary>
        /// Returns all available cameras 
        /// </summary>
        public IList<Camera> Cameras
        {
            get
            {
                if (_cameras == null)
                    RefreshCameraList();

                return _cameras.AsReadOnly();
            }
        }

        /// <summary>
        /// The camera currently acquiring images, only one camera can be acquiring at one time and 
        /// its starts acquiring as soon as it is set
        /// </summary>
        /// <example>
        /// <code>
        ///     TouchlessMgr _touch = new TouchlessMgr();
        ///     ...
        /// 
        ///     // Pick the first available camera 
        ///     _touch.CurrentCamera = _touch.Cameras[0];
        /// </code>
        /// </example>
        public Camera CurrentCamera
        {
            get { return _currentCamera; }
            set
            {
                if (value == null || value == _currentCamera)
                    return;

                if (_currentCamera != null)
                {
                    Camera._CameraMethods.StopCamera();
                    _currentCamera = null;
                }

                _nativeCallback = new WebCamCaptureCallback(CaptureCallbackProc);
                Camera._CameraMethods.OnImageCapture += new CameraMethods.CaptureCallbackDelegate(_nativeCallback);
                Camera._CameraMethods.StartCamera(value.Index, ref _camWidth, ref _camHeight);
                _currentCamera = value;
            }
        }

        /// <summary>
        /// Returns all tracked markers as an enumeration
        /// </summary>
        public IList<Marker> Markers
        {
            get { return _markers.AsReadOnly(); }
        }

        /// <summary>
        /// Get the current count of tracked markers
        /// </summary>
        public int MarkerCount
        {
            get { return _markers.Count; }
        }

        /// <summary>
        /// Adds a new marker given training data as an image and proposed marker region
        /// </summary>
        /// <param name="name">Caller supplied name for the new marker</param>
        /// <param name="image">The image to scan for the new marker</param>
        /// <param name="center">The center point of the new marker as pixel coordinates</param>
        /// <param name="radius">The radius of the new marker in pixels</param>
        /// <returns>A newly trained marker</returns>
        /// /// <example>
        /// The following is a code snippet that shows how to create a Marker
        /// <code>
        ///     TouchlessMgr _touch = new TouchlessMgr();
        ///     ...
        /// 
        ///     // Create a new marker using the current image at a fixed location and size
        ///     Marker m = _touch.AddMarker("marker", _touch.CurrentCamera.GetCurrentImage(), new Point(100, 100), 10);
        /// </code>
        /// </example>
        public Marker AddMarker(string name, Bitmap image, Point center, float radius)
        {
            Marker newMarker = new Marker(name);
            newMarker.SetMarkerAppearance(GetMarkerAppearance(image, center, radius));
            _markers.Add(newMarker);
            return newMarker;
        }

        /// <summary>
        /// Removes a currently active marker and stops all tracking
        /// </summary>
        /// <param name="index">Index of the marker to remove</param>
        public void RemoveMarker(int index)
        {
            if (index < 0 || index >= _markers.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            _markers.RemoveAt(index);
        }

        /// <summary>
        /// The default contructor for a new TouchlessMgr
        /// </summary>
        public TouchlessMgr()
        {
            _markers = new List<Marker>();
            // Initialize the per-axis lists of marker scan commands
            _markerScanCommandsX = new List<MarkerScanCommand>();
            _markerScanCommandsY = new List<MarkerScanCommand>();
            // Initialize the per-axis lists of markers to consider in the image loop
            _scanMarkersX = new List<Marker>();
            _scanMarkersY = new List<Marker>();
            // Initialize the marker scan command comparison function delegate
            _MarkerScanCommandComparison = new Comparison<MarkerScanCommand>(delegate(MarkerScanCommand msc1, MarkerScanCommand msc2) { return msc1.coordinate - msc2.coordinate; });
        }

        #endregion Public Interface

        #region Internal Implementation

        /// <summary>
        /// Callback delegate invoked by DirectShow to deliver a frame from the webcam
        /// </summary>
        private delegate void WebCamCaptureCallback(
                    int dwSize,
                    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 0)] byte[] abData);

        // Cameras
        private List<Camera> _cameras = null;
        // Needed to avoid garbage collection problem
        private WebCamCaptureCallback _nativeCallback;
        private Camera _currentCamera = null;
        private int _camWidth = 0;
        private int _camHeight = 0;

        // Markers
        private List<Marker> _markers = null;
        // Per-axis lists of marker scan commands
        private List<MarkerScanCommand> _markerScanCommandsX = null;
        private List<MarkerScanCommand> _markerScanCommandsY = null;
        // The per-axis lists of markers to consider in the image loop
        private List<Marker> _scanMarkersX = null;
        private List<Marker> _scanMarkersY = null;
        // The marker scan command comparison function delegate
        private Comparison<MarkerScanCommand> _MarkerScanCommandComparison;

        /// <summary>
        /// Here is where the images come in as they are collected, as fast as they can and on a background thread
        /// </summary>
        internal void CaptureCallbackProc(int dataSize, byte[] data)
        {
            if (_currentCamera != null)
            {
                // Do the magic to create a bitmap
                int stride = _camWidth * 3;
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                int scan0 = (int)handle.AddrOfPinnedObject();
                scan0 += (_camHeight - 1) * stride;
                Bitmap b = new Bitmap(_camWidth, _camHeight, -stride, PixelFormat.Format24bppRgb, (IntPtr)scan0);

                // Now you can free the handle
                handle.Free();

                // Copy the image using the Thumbnail function to also resize if needed
                Bitmap copyBitmap = (Bitmap)b.GetThumbnailImage(_currentCamera.CaptureWidth, _currentCamera.CaptureHeight, null, IntPtr.Zero);

                // Perform the needed rotate/flip on the image
                copyBitmap.RotateFlip(_currentCamera.RotateFlip);

                // Process the image to update the marker data
                UpdateMarkers(copyBitmap);

                // pass it to the camera for its events and processing
                _currentCamera.ImageCaptured(copyBitmap);
            }
        }

        /// <summary>
        /// Get a marker's color using the training data of an image and a suggested marker circle
        /// This version provides default bin counts to the core function
        /// </summary>
        /// <param name="image">The image to be analyzed for marker color</param>
        /// <param name="center">The center of the proposed marker in pixel coordinates</param>
        /// <param name="radius">The radius of the proposed marker in pixels</param>
        /// <returns></returns>
        internal int[, ,] GetMarkerAppearance(Bitmap image, Point center, float radius)
        {
            // TODO: Improve partitioning, consider using: binCounts = new HSV(20, 20, 10)
            return GetMarkerAppearance(image, center, radius, new HSV(40, 20, 10));
        }

        /// <summary>
        /// Get a marker's color using the training data of an image and a suggested marker circle
        /// </summary>
        /// <param name="image">The image to be analyzed for marker color</param>
        /// <param name="center">The center of the proposed marker in pixel coordinates</param>
        /// <param name="radius">The radius of the proposed marker in pixels</param>
        /// <param name="binCounts">The number of bins per dimension used to partition hsv colors</param>
        internal int[, ,] GetMarkerAppearance(Bitmap image, Point center, float radius, HSV binCounts)
        {
            int pixelCount = image.Height * image.Width;
            int[, ,] hsvFreq = new int[binCounts.H, binCounts.S, binCounts.V];

            unsafe
            {
                // TODO: Validate image.PixelFormat
                // TODO: push this to a function like Camera.getImageData();
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData data = image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                int* pPixel = (int*)data.Scan0;
                HSV hsv;
                int pointDx, pointDy;
                bool inMarkerRegion = false;

                // Accumulate pixels from inside and outside the proposed marker region
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        // TODO: Remove indirection....
                        // Align the pixel color in HSV with its appropriate HSV bins
                        hsv = HSV.GetBinnedHSV(RGB.ConvertToHSV(new RGB(*(pPixel++))), binCounts);

                        // Is the point within the suggested marker circle?
                        // TODO: optimize for line by line processing (calculate the points of intersection per line...)
                        pointDx = x - center.X;
                        pointDy = y - center.Y;
                        inMarkerRegion = Math.Sqrt(pointDx * pointDx + pointDy * pointDy) < radius;

                        // Increment/decrement table [-1, 2]
                        // TODO: alter the increment/decrement values to accomodate signal/noise ratio
                        hsvFreq[hsv.H, hsv.S, hsv.V] += inMarkerRegion ? 2 : -1;
                    }
                }

                // Unlock the image data
                image.UnlockBits(data);
            }

            return hsvFreq;
        }

        /// <summary>
        /// Update information about the current collection of markers
        /// </summary>
        /// <param name="image">Used as input to update markers, and as output for image adornments</param>
        internal void UpdateMarkers(Bitmap image)
        {
            if (_markers == null || _markers.Count == 0)
                return;

            // Allow use of pointers
            unsafe
            {
                // TODO: Validate image.PixelFormat
                // TODO: push this to a function like Camera.getImageData();
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData data = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
                int* pPixel0 = (int*)data.Scan0, pPixelCurr;

                RGB rgb = new RGB();
                HSV hsv = new HSV();
                ColorKey hsvkey = new ColorKey();
                uint freq = 0;

                // Make a copy of _markers for thread safety
                // TODO: this is NOT thread safe. for real thread safety, use lock(_markers) or another such mechanism
                Marker[] markers = new Marker[_markers.Count];
                _markers.CopyTo(0, markers, 0, markers.Length);

                // Prepare the markers for update and populate the y-axis scan command list
                // TODO: smarter updating of command lists between consecutive images?
                _markerScanCommandsY.Clear();
                foreach (Marker marker in _markers)
                {
                    if (marker.Active && marker.framesToSkip == 0)
                    {
                        preProcessMarker(marker, image.Width, image.Height);
                        _markerScanCommandsY.Add(marker.searchMinY);
                        _markerScanCommandsY.Add(marker.searchMaxY);
                    }
                }

                // Sort the y-axis scan command list
                _markerScanCommandsY.Sort(_MarkerScanCommandComparison);

                // Loop over the y-axis scan command list
                for (int mscIndexY = 0; mscIndexY < _markerScanCommandsY.Count; mscIndexY++)
                {
                    // Add or remove the marker for consideration in the x-axis command list
                    if (_markerScanCommandsY[mscIndexY].command == ScanCommand.addMarker)
                        _scanMarkersY.Add(_markerScanCommandsY[mscIndexY].marker);
                    else /*if(_markerScanCommandsY[mscIndexY].scanCommand == ScanCommand.remMarker)*/
                        _scanMarkersY.Remove(_markerScanCommandsY[mscIndexY].marker);

                    // If we have no markers to scan in this set of rows, jump to the next y-axis scan command (continue)
                    if (_scanMarkersY.Count == 0)
                        continue;

                    // Populate the x-axis scan command list from the y-axis list of markers for consideration
                    // TODO: smarter updating of command lists between row sections?
                    _markerScanCommandsX.Clear();
                    foreach (Marker marker in _scanMarkersY)
                    {
                        _markerScanCommandsX.Add(marker.searchMinX);
                        _markerScanCommandsX.Add(marker.searchMaxX);
                    }

                    // Sort the x-axis scan command list
                    _markerScanCommandsX.Sort(_MarkerScanCommandComparison);

                    // Loop over the x-axis scan command list
                    for (int mscIndexX = 0; mscIndexX < _markerScanCommandsX.Count; mscIndexX++)
                    {
                        // Add or remove the marker for consideration in the pixel scanning
                        if (_markerScanCommandsX[mscIndexX].command == ScanCommand.addMarker)
                            _scanMarkersX.Add(_markerScanCommandsX[mscIndexX].marker);
                        else /*if(_markerScanCommandsX[mscIndexX].scanCommand == ScanCommand.remMarker)*/
                            _scanMarkersX.Remove(_markerScanCommandsX[mscIndexX].marker);

                        // If we have no markers to scan in this area of the row, jump to the next x-axis scan command (continue)
                        if (_scanMarkersX.Count == 0)
                            continue;

                        // Loop over the next solid area
                        for (int y = _markerScanCommandsY[mscIndexY].coordinate; y <= _markerScanCommandsY[mscIndexY + 1].coordinate; y++)
                        {
                            pPixelCurr = pPixel0 + image.Width * y + _markerScanCommandsX[mscIndexX].coordinate;
                            for (int x = _markerScanCommandsX[mscIndexX].coordinate; x <= _markerScanCommandsX[mscIndexX + 1].coordinate; x++, pPixelCurr++)
                            {
                                // Get the pixel color in HSV
                                // TODO: Remove indirection
                                rgb = new RGB(*pPixelCurr);
                                hsv = RGB.ConvertToHSV(rgb);

                                foreach(Marker marker in _scanMarkersX)
                                {
                                    // Align the color with its appropriate bins, get color key
                                    hsvkey.Hsv = HSV.GetBinnedHSV(hsv, marker.bins);
                                    if (marker.hsvFreq.TryGetValue(hsvkey, out freq) && freq > marker.Threshold)
                                    {
                                        marker.currData.Area++;
                                        marker.currData.X += x;
                                        marker.currData.Y += y;
                                        if (y < marker.currData.Top) marker.currData.Top = y;
                                        if (y > marker.currData.Bottom) marker.currData.Bottom = y;
                                        if (x < marker.currData.Left) marker.currData.Left = x;
                                        if (x > marker.currData.Right) marker.currData.Right = x;

                                        // Aggregate the average visible RGB color
                                        marker.avgR += rgb.R;
                                        marker.avgG += rgb.G;
                                        marker.avgB += rgb.B;

                                        if (marker.Highlight)
                                            *pPixelCurr = marker.representativeRGB;
                                    }
                                }
                            }
                        }
                    }
                }// End loop over image

                // Draw the per-marker search bounds
                int* pPixelA, pPixelB;
                foreach (Marker marker in markers)
                {
                    if (marker.Active && marker.framesToSkip == 0 && marker.Highlight && marker.currData.Present)
                    {
                        pPixelA = pPixel0 + image.Width * marker.searchMinY.coordinate + marker.searchMinX.coordinate;
                        pPixelB = pPixel0 + image.Width * marker.searchMaxY.coordinate + marker.searchMinX.coordinate;
                        for (int x = marker.searchMinX.coordinate; x <= marker.searchMaxX.coordinate; x++, pPixelA++, pPixelB++)
                            *pPixelA = *pPixelB = marker.representativeRGB;

                        pPixelA = pPixel0 + image.Width * marker.searchMinY.coordinate + marker.searchMinX.coordinate;
                        pPixelB = pPixel0 + image.Width * marker.searchMinY.coordinate + marker.searchMaxX.coordinate;
                        for (int y = marker.searchMinY.coordinate; y <= marker.searchMaxY.coordinate; y++, pPixelA += image.Width, pPixelB += image.Width)
                            *pPixelA = *pPixelB = marker.representativeRGB;
                    }
                }

                // Unlock the image data
                image.UnlockBits(data);

                // Perform additional computation and post-processing on the marker
                foreach (Marker marker in markers)
                {
                    if (marker.Active && marker.framesToSkip == 0)
                        postProcessMarker(marker);
                    else if (marker.framesToSkip > 0)
                        marker.framesToSkip--;
                }
            }
        }

        /// <summary>
        /// Perform preparatory action on a marker before update
        /// </summary>
        /// <param name="marker">The marker to be processed</param>
        /// <param name="imageWidth">The width of the image image being processed</param>
        /// <param name="imageHeight">The height of the image image being processed</param>
        internal void preProcessMarker(Marker marker, int imageWidth, int imageHeight)
        {
            // Store the previous/lastgood data and intialize new data
            if (marker.currData.Present)
                marker.lastGoodData = marker.currData;
            marker.prevData = marker.currData;
            marker.currData = new MarkerEventData();

            // Set the search bounds
            if (marker.prevData.Present)
            {
                // TODO: Improve the search bounds, consider acceleration...
                int xBuffer = imageWidth / 20;
                int yBuffer = imageHeight / 20;
                // Map [-dx|y, +dx|y] to [1/4, 4] for a motion prediction factor
                float dxFactor = (marker.prevData.DX < 0) ? 0.25F : 4;
                float dyFactor = (marker.prevData.DY < 0) ? 0.25F : 4;
                marker.searchMinX.coordinate = (int)(marker.prevData.Left   + marker.prevData.DX / dxFactor - marker.prevData.Width  / 3 - xBuffer);
                marker.searchMaxX.coordinate = (int)(marker.prevData.Right  + marker.prevData.DX * dxFactor + marker.prevData.Width  / 3 + xBuffer);
                marker.searchMinY.coordinate = (int)(marker.prevData.Top    + marker.prevData.DY / dyFactor - marker.prevData.Height / 3 - yBuffer);
                marker.searchMaxY.coordinate = (int)(marker.prevData.Bottom + marker.prevData.DY * dyFactor + marker.prevData.Height / 3 + yBuffer);

                // Ensure the marker search bounds are within the image region
                marker.searchMinX.coordinate = Math.Max(marker.searchMinX.coordinate, 0);
                marker.searchMaxX.coordinate = Math.Min(marker.searchMaxX.coordinate, imageWidth - 1);
                marker.searchMinY.coordinate = Math.Max(marker.searchMinY.coordinate, 0);
                marker.searchMaxY.coordinate = Math.Min(marker.searchMaxY.coordinate, imageHeight - 1);
            }
            else
            {
                marker.searchMinX.coordinate = marker.searchMinY.coordinate = 0;
                marker.searchMaxX.coordinate = imageWidth - 1;
                marker.searchMaxY.coordinate = imageHeight - 1;
            }

            // Initialize the new marker bounds values
            marker.currData.Top = imageHeight;
            marker.currData.Bottom = 0;
            marker.currData.Left = imageWidth;
            marker.currData.Right = 0;

            // Initalize the average visible RGB color
            marker.avgR = marker.avgG = marker.avgB = 0;
        }

        /// <summary>
        /// Perform additional computation and post-processing on the marker
        /// </summary>
        /// <param name="marker">The marker to be processed</param>
        internal void postProcessMarker(Marker marker)
        {
            marker.currData.Timestamp = DateTime.Now;

            // If the marker was found
            if (marker.currData.Present)
            {
                marker.currData.X /= marker.currData.Area;
                marker.currData.Y /= marker.currData.Area;

                if (marker.prevData.Present)
                {
                    marker.currData.DX = marker.currData.X - marker.prevData.X;
                    marker.currData.DY = marker.currData.Y - marker.prevData.Y;
                }

                // Calculate the average rgb color
                marker.RepresentativeColor = Color.FromArgb((byte)(marker.avgR / marker.currData.Area),
                                                            (byte)(marker.avgG / marker.currData.Area),
                                                            (byte)(marker.avgB / marker.currData.Area));
                marker.representativeRGB = marker.RepresentativeColor.ToArgb();

                // Perform smoothing
                if (marker.SmoothingEnabled)
                {
                    // Blend the weighted average center with the bounding box center
                    marker.currData.X = (marker.currData.X + (marker.currData.Left + marker.currData.Right) / 2.0) / 2.0;
                    marker.currData.Y = (marker.currData.Y + (marker.currData.Top + marker.currData.Bottom) / 2.0) / 2.0;

                    if (marker.prevData.Present)
                    {
                        // Smoothing factors
                        float alpha = marker.smoothingFactor;
                        float beta = 1 - alpha;

                        // Smooth the position and area with the previous data
                        marker.currData.Area = (int)(alpha * marker.currData.Area + beta * marker.prevData.Area);
                        marker.currData.X = alpha * marker.currData.X + beta * marker.prevData.X;
                        marker.currData.Y = alpha * marker.currData.Y + beta * marker.prevData.Y;

                        // Smooth the position delta with the previous data
                        marker.currData.DX = marker.currData.X - marker.prevData.X;
                        marker.currData.DY = marker.currData.Y - marker.prevData.Y;

                        // Smooth the bounds with the previous data
                        marker.currData.Top = alpha * marker.currData.Top + beta * marker.prevData.Top;
                        marker.currData.Bottom = alpha * marker.currData.Bottom + beta * marker.prevData.Bottom;
                        marker.currData.Left = alpha * marker.currData.Left + beta * marker.prevData.Left;
                        marker.currData.Right = alpha * marker.currData.Right + beta * marker.prevData.Right;
                    }
                }
            }

            // Fire an event to notify handlers that the marker data was updated
            if (marker.currData.Present || marker.currData.Present != marker.prevData.Present)
                marker.FireOnChangeEvent();

            // Skip some frames if the marker was not found
            // TODO: Adjust value to accomodate perf/responsiveness
            if (!marker.currData.Present)
                marker.framesToSkip += 5;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleanup function for the library
        /// </summary>
        public void Dispose()
        {
            if (_currentCamera != null)
            {
                Camera._CameraMethods.StopCamera();
                _currentCamera = null;
            }

            if (_cameras != null)
            {
                _cameras.Clear();
                _cameras = null;
            }

            if (_markers != null)
            {
                _markers.Clear();
                _markers = null;
            }
        }

        #endregion
    }
}
