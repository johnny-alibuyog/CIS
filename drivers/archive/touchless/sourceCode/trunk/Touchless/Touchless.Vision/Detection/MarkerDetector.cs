using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;
using Touchless.Vision.Detection.Configuration;
using Touchless.Vision.Tracking;
using Point=System.Drawing.Point;

namespace Touchless.Vision.Detection
{
    [Export(typeof(IObjectDetector))]
    public class MarkerDetector : IObjectDetector
    {
        private readonly List<Marker> _markers;
        private readonly ObjectTrackingService _objectTrackingService;

        public MarkerDetector()
        {
            _markers = new List<Marker>();
            _objectTrackingService = new ObjectTrackingService();
            _objectTrackingService.NewObject += (s, o, f) => NewObject.IfNotNull(i => i(this, o, f));
            _objectTrackingService.ObjectMoved += (s, o, f) => ObjectMoved.IfNotNull(i => i(this, o, f));
            _objectTrackingService.ObjectRemoved += (s, o, f) => OnObjectRemoved(o, f);
        }

        public ReadOnlyCollection<Marker> TrackingMarkers
        {
            get
            {
                return _markers.AsReadOnly();
            }
        }

        private void OnObjectRemoved(DetectedObject detectedObject, Frame frame)
        {
            ObjectRemoved.IfNotNull(i => i(this, detectedObject, frame));
            (detectedObject as Marker).IfNotNull(i => i.AssignNewId());
        }

        #region IObjectDetector Members

        public event Action<IObjectDetector, DetectedObject, Frame> NewObject , ObjectMoved , ObjectRemoved;
        public event Action<IObjectDetector, Frame, ReadOnlyCollection<DetectedObject>> FrameProcessed;

        public ReadOnlyCollection<DetectedObject> DetectObjects(Frame frame)
        {
            UpdateMarkers(frame.Image);
            ReadOnlyCollection<DetectedObject> detectedMarkers = _markers
                .Where(i => i.currData.Present)
                .Select(i => i as DetectedObject)
                .ToList()
                .AsReadOnly();

            if (FrameProcessed != null)
            {
                FrameProcessed(this, frame, detectedMarkers);
            }

            _objectTrackingService.UpdateDetectedObjects(frame, detectedMarkers);

            return detectedMarkers;
        }

        public string Name
        {
            get { return "Touchless Marker Detector"; }
        }

        public string Description
        {
            get { return "Uses color matching to detect objects on screen."; }
        }

        public bool HasConfiguration
        {
            get { return true; }
        }

        public UIElement ConfigurationElement
        {
            get
            {
                return new MarkerDetectorConfigurationElement(this);
            }
        }

        #endregion

        public Marker AddMarker(string name, Bitmap image, Point center, float radius)
        {
            var newMarker = new Marker(name);
            newMarker.Image = image;
            newMarker.SetMarkerAppearance(GetMarkerAppearance(image, center, radius));
            _markers.Add(newMarker);

            return newMarker;
        }

        public void RemoveMarker(int index)
        {
            _markers.RemoveAt(index);
        }


        /// <summary>
        /// Update information about the current collection of markers
        /// </summary>
        /// <param name="image">Used as input to update markers, and as output for image adornments</param>
        private void UpdateMarkers(Bitmap image)
        {
            var markerScanCommandsY = new List<MarkerScanCommand>();
            var markerScanCommandsX = new List<MarkerScanCommand>();
            var scanMarkersX = new List<Marker>();
            var scanMarkersY = new List<Marker>();
            var markerScanCommandComparison =
                new Comparison<MarkerScanCommand>(
                    delegate(MarkerScanCommand msc1, MarkerScanCommand msc2) { return msc1.coordinate - msc2.coordinate; });

            if (_markers == null || _markers.Count == 0)
                return;

            // Allow use of pointers
            unsafe
            {
                // TODO: Validate image.PixelFormat
                // TODO: push this to a function like Camera.getImageData();
                var rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData data = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
                int* pPixel0 = (int*) data.Scan0, pPixelCurr;

                var rgb = new RGB();
                var hsv = new HSV();
                var hsvkey = new ColorKey();
                uint freq = 0;

                // Make a copy of _markers for thread safety
                // TODO: this is NOT thread safe. for real thread safety, use lock(_markers) or another such mechanism
                var markers = new Marker[_markers.Count];
                _markers.CopyTo(0, markers, 0, markers.Length);

                // Prepare the markers for update and populate the y-axis scan command list
                // TODO: smarter updating of command lists between consecutive images?
                markerScanCommandsY.Clear();
                foreach (Marker marker in _markers)
                {
                    if (marker.Active && marker.framesToSkip == 0)
                    {
                        preProcessMarker(marker, image.Width, image.Height);
                        markerScanCommandsY.Add(marker.searchMinY);
                        markerScanCommandsY.Add(marker.searchMaxY);
                    }
                }

                // Sort the y-axis scan command list
                markerScanCommandsY.Sort(markerScanCommandComparison);

                // Loop over the y-axis scan command list
                for (int mscIndexY = 0; mscIndexY < markerScanCommandsY.Count; mscIndexY++)
                {
                    // Add or remove the marker for consideration in the x-axis command list
                    if (markerScanCommandsY[mscIndexY].command == ScanCommand.addMarker)
                        scanMarkersY.Add(markerScanCommandsY[mscIndexY].marker);
                    else /*if(_markerScanCommandsY[mscIndexY].scanCommand == ScanCommand.remMarker)*/
                        scanMarkersY.Remove(markerScanCommandsY[mscIndexY].marker);

                    // If we have no markers to scan in this set of rows, jump to the next y-axis scan command (continue)
                    if (scanMarkersY.Count == 0)
                        continue;

                    // Populate the x-axis scan command list from the y-axis list of markers for consideration
                    // TODO: smarter updating of command lists between row sections?
                    markerScanCommandsX.Clear();
                    foreach (Marker marker in scanMarkersY)
                    {
                        markerScanCommandsX.Add(marker.searchMinX);
                        markerScanCommandsX.Add(marker.searchMaxX);
                    }

                    // Sort the x-axis scan command list
                    markerScanCommandsX.Sort(markerScanCommandComparison);

                    // Loop over the x-axis scan command list
                    for (int mscIndexX = 0; mscIndexX < markerScanCommandsX.Count; mscIndexX++)
                    {
                        // Add or remove the marker for consideration in the pixel scanning
                        if (markerScanCommandsX[mscIndexX].command == ScanCommand.addMarker)
                            scanMarkersX.Add(markerScanCommandsX[mscIndexX].marker);
                        else /*if(_markerScanCommandsX[mscIndexX].scanCommand == ScanCommand.remMarker)*/
                            scanMarkersX.Remove(markerScanCommandsX[mscIndexX].marker);

                        // If we have no markers to scan in this area of the row, jump to the next x-axis scan command (continue)
                        if (scanMarkersX.Count == 0)
                            continue;

                        // Loop over the next solid area
                        for (int y = markerScanCommandsY[mscIndexY].coordinate;
                             y <= markerScanCommandsY[mscIndexY + 1].coordinate;
                             y++)
                        {
                            pPixelCurr = pPixel0 + image.Width*y + markerScanCommandsX[mscIndexX].coordinate;
                            for (int x = markerScanCommandsX[mscIndexX].coordinate;
                                 x <= markerScanCommandsX[mscIndexX + 1].coordinate;
                                 x++, pPixelCurr++)
                            {
                                // Get the pixel color in HSV
                                // TODO: Remove indirection
                                rgb = new RGB(*pPixelCurr);
                                hsv = RGB.ConvertToHSV(rgb);

                                foreach (Marker marker in scanMarkersX)
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
                } // End loop over image

                // Draw the per-marker search bounds
                int* pPixelA, pPixelB;
                foreach (Marker marker in markers)
                {
                    if (marker.Active && marker.framesToSkip == 0 && marker.Highlight && marker.currData.Present)
                    {
                        pPixelA = pPixel0 + image.Width*marker.searchMinY.coordinate + marker.searchMinX.coordinate;
                        pPixelB = pPixel0 + image.Width*marker.searchMaxY.coordinate + marker.searchMinX.coordinate;
                        for (int x = marker.searchMinX.coordinate;
                             x <= marker.searchMaxX.coordinate;
                             x++, pPixelA++, pPixelB++)
                            *pPixelA = *pPixelB = marker.representativeRGB;

                        pPixelA = pPixel0 + image.Width*marker.searchMinY.coordinate + marker.searchMinX.coordinate;
                        pPixelB = pPixel0 + image.Width*marker.searchMinY.coordinate + marker.searchMaxX.coordinate;
                        for (int y = marker.searchMinY.coordinate;
                             y <= marker.searchMaxY.coordinate;
                             y++, pPixelA += image.Width, pPixelB += image.Width)
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
                int xBuffer = imageWidth/20;
                int yBuffer = imageHeight/20;
                // Map [-dx|y, +dx|y] to [1/4, 4] for a motion prediction factor
                float dxFactor = (marker.prevData.DX < 0) ? 0.25F : 4;
                float dyFactor = (marker.prevData.DY < 0) ? 0.25F : 4;
                marker.searchMinX.coordinate =
                    (int) (marker.prevData.Left + marker.prevData.DX/dxFactor - marker.prevData.Width/3 - xBuffer);
                marker.searchMaxX.coordinate =
                    (int) (marker.prevData.Right + marker.prevData.DX*dxFactor + marker.prevData.Width/3 + xBuffer);
                marker.searchMinY.coordinate =
                    (int) (marker.prevData.Top + marker.prevData.DY/dyFactor - marker.prevData.Height/3 - yBuffer);
                marker.searchMaxY.coordinate =
                    (int) (marker.prevData.Bottom + marker.prevData.DY*dyFactor + marker.prevData.Height/3 + yBuffer);

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
                marker.RepresentativeColor = Color.FromArgb((byte) (marker.avgR/marker.currData.Area),
                                                            (byte) (marker.avgG/marker.currData.Area),
                                                            (byte) (marker.avgB/marker.currData.Area));
                marker.representativeRGB = marker.RepresentativeColor.ToArgb();

                // Perform smoothing
                if (marker.SmoothingEnabled)
                {
                    // Blend the weighted average center with the bounding box center
                    marker.currData.X = (marker.currData.X + (marker.currData.Left + marker.currData.Right)/2.0)/2.0;
                    marker.currData.Y = (marker.currData.Y + (marker.currData.Top + marker.currData.Bottom)/2.0)/2.0;

                    if (marker.prevData.Present)
                    {
                        // Smoothing factors
                        float alpha = marker.smoothingFactor;
                        float beta = 1 - alpha;

                        // Smooth the position and area with the previous data
                        marker.currData.Area = (int) (alpha*marker.currData.Area + beta*marker.prevData.Area);
                        marker.currData.X = alpha*marker.currData.X + beta*marker.prevData.X;
                        marker.currData.Y = alpha*marker.currData.Y + beta*marker.prevData.Y;

                        // Smooth the position delta with the previous data
                        marker.currData.DX = marker.currData.X - marker.prevData.X;
                        marker.currData.DY = marker.currData.Y - marker.prevData.Y;

                        // Smooth the bounds with the previous data
                        marker.currData.Top = alpha*marker.currData.Top + beta*marker.prevData.Top;
                        marker.currData.Bottom = alpha*marker.currData.Bottom + beta*marker.prevData.Bottom;
                        marker.currData.Left = alpha*marker.currData.Left + beta*marker.prevData.Left;
                        marker.currData.Right = alpha*marker.currData.Right + beta*marker.prevData.Right;
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

        /// <summary>
        /// Get a marker's color using the training data of an image and a suggested marker circle
        /// This version provides default bin counts to the core function
        /// </summary>
        /// <param name="image">The image to be analyzed for marker color</param>
        /// <param name="center">The center of the proposed marker in pixel coordinates</param>
        /// <param name="radius">The radius of the proposed marker in pixels</param>
        /// <returns></returns>
        internal int[,,] GetMarkerAppearance(Bitmap image, Point center, float radius)
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
        internal int[,,] GetMarkerAppearance(Bitmap image, Point center, float radius, HSV binCounts)
        {
            int pixelCount = image.Height*image.Width;
            var hsvFreq = new int[binCounts.H,binCounts.S,binCounts.V];

            unsafe
            {
                // TODO: Validate image.PixelFormat
                // TODO: push this to a function like Camera.getImageData();
                var rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData data = image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                var pPixel = (int*) data.Scan0;
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
                        inMarkerRegion = Math.Sqrt(pointDx*pointDx + pointDy*pointDy) < radius;

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
    }
}