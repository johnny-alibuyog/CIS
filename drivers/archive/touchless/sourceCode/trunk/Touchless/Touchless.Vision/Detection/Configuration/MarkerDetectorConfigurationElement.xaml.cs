using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Touchless.Shared.Extensions;
using Frame=Touchless.Vision.Contracts.Frame;
using Point=System.Drawing.Point;

namespace Touchless.Vision.Detection.Configuration
{
    /// <summary>
    /// Interaction logic for MarkerDetectorConfigurationElement.xaml
    /// </summary>
    public partial class MarkerDetectorConfigurationElement : UserControl
    {
        private readonly MarkerDetector _markerDetector;
        private readonly Stopwatch _uiUpdateTimer;
        private readonly Bitmap _drawingCanvas;
        private int _addedMarkerCount;
        private bool _drawSelectionAdornment;
        private bool _fAddingMarker;
        private Bitmap _latestFrame;
        private Point _markerCenter;
        private float _markerRadius;
        private Marker _markerSelected;

        public MarkerDetectorConfigurationElement()
            : this(null)
        {
        }

        public MarkerDetectorConfigurationElement(MarkerDetector markerDetector)
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                // Initialize some members
                _fAddingMarker = false;
                _markerSelected = null;
                _addedMarkerCount = 0;
                _uiUpdateTimer = new Stopwatch();

                // Set or unset the picturebox mouse interaction handlers
                _drawingCanvas = new Bitmap((int) pictureBoxDisplay.Width, (int) pictureBoxDisplay.Height);
                _markerDetector = markerDetector;
                _markerDetector.FrameProcessed += (o, f, d) => OnImageCaptured(f);

                Loaded += (s, e) => _uiUpdateTimer.Start();
            }
        }

        #region Handle Images

        private void DrawLatestImage()
        {
            if (_latestFrame != null)
            {
                Graphics g = Graphics.FromImage(_drawingCanvas);

                // Draw the latest image from the active camera
                g.DrawImage(_latestFrame, 0, 0, _drawingCanvas.Width, _drawingCanvas.Height);

                // Draw the selection adornment
                if (_drawSelectionAdornment)
                    g.DrawEllipse(new Pen(Brushes.Red, 1), _markerCenter.X - _markerRadius,
                                  _markerCenter.Y - _markerRadius, 2*_markerRadius, 2*_markerRadius);

                pictureBoxDisplay.Source = _drawingCanvas.ToBitmapSource();
            }
        }

        /// <summary>
        /// Event handler from the active camera
        /// </summary>
        public void OnImageCaptured(Frame frame)
        {
            // Update the FPS display once every second
            if (_uiUpdateTimer.ElapsedMilliseconds >= 1000)
            {
                //this.BeginInvoke(new Action<double>(UpdateFPSInUI), new object[] { fps });
                _uiUpdateTimer.Reset();
                _uiUpdateTimer.Start();
            }

            // Save the latest image for drawing
            if (!_fAddingMarker)
            {
                // Cause display Update
                _latestFrame = frame.Image;
                //pictureBoxDisplay.Invalidate();
            }

            Dispatcher.BeginInvoke((Action) DrawLatestImage);
        }

        /// <summary>
        /// Event Handler from the selected marker in the Marker Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnSelectedMarkerUpdate(object sender, MarkerEventArgs args)
        {
            Action workAction = () => UpdateMarkerDataInUI(args.EventData);
            Dispatcher.BeginInvoke(workAction);
        }

        private void UpdateMarkerDataInUI(MarkerEventData data)
        {
            labelMarkerData.Text =
                "Center X:  " + (int) data.X + "\n"
                + "Center Y:  " + (int) data.Y + "\n"
                + "DX:        " + (int) data.DX + "\n"
                + "DY:        " + (int) data.DY + "\n"
                + "Area:      " + (int) data.Area + "\n"
                + "Left:      " + (int) data.Left + "\n"
                + "Right:     " + (int) data.Right + "\n"
                + "Top:       " + (int) data.Top + "\n"
                + "Bottom:    " + (int) data.Bottom + "\n"
                + "Width:     " + (int) data.Width + "\n"
                + "Height:    " + (int) data.Height + "\n";
        }

        #endregion Event Handling


        #region Event Handlers Galore
        private void buttonMarkerAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _fAddingMarker = !_fAddingMarker;
            buttonMarkerAdd.Content = _fAddingMarker ? "Cancel" : "Add Marker";
        }

        private void comboBoxMarkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_markerSelected != null)
                _markerSelected.OnChange -= new EventHandler<MarkerEventArgs>(OnSelectedMarkerUpdate);

            if (comboBoxMarkers.SelectedIndex < 0)
            {
                comboBoxMarkers.Text = "Select Marker";
                groupBoxMarkerControl.IsEnabled = false;
                groupBoxMarkerControl.Header = "No Marker Selected";
                return;
            }

            _markerSelected = (Marker) comboBoxMarkers.SelectedItem;
            _markerSelected.OnChange += OnSelectedMarkerUpdate;

            groupBoxMarkerControl.Header = _markerSelected.Name;
            groupBoxMarkerControl.IsEnabled = true;
            checkBoxMarkerActive.IsChecked = _markerSelected.Active;
            checkBoxMarkerHighlight.IsChecked = _markerSelected.Highlight;
            checkBoxMarkerSmoothing.IsChecked = _markerSelected.SmoothingEnabled;
            txtThreshold.Text = _markerSelected.Threshold.ToString();
        }

        private void buttonMarkerRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (comboBoxMarkers.SelectedIndex < 0)
                return;

            _markerDetector.RemoveMarker(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.Items.RemoveAt(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.SelectedIndex = -1;
            comboBoxMarkers.Text = "Select Marker";
            groupBoxMarkerControl.IsEnabled = false;
            groupBoxMarkerControl.Header = "No Marker Selected";
            if (comboBoxMarkers.Items.Count == 0)
            {
                comboBoxMarkers.IsEnabled = false;
            }
        }

        private void pictureBoxDisplay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // If we are adding a marker - get the marker center on mouse down
            if (_fAddingMarker)
            {
                _markerCenter = e.GetPosition(pictureBoxDisplay).ToDrawingPoint();
                _markerRadius = 0;

                // Begin drawing the selection adornment
                _drawSelectionAdornment = true;
            }
        }

        private void pictureBoxDisplay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // If we are adding a marker - get the marker radius on mouse up, add the marker
            if (_fAddingMarker)
            {
                Point position = e.GetPosition(pictureBoxDisplay).ToDrawingPoint();
                int dx = position.X - _markerCenter.X;
                int dy = position.Y - _markerCenter.Y;
                _markerRadius = (float) Math.Sqrt(dx*dx + dy*dy);
                // Adjust for the image/display scaling (assumes proportional scaling)
                _markerCenter.X = (_markerCenter.X*_latestFrame.Width)/_drawingCanvas.Width;
                _markerCenter.Y = (_markerCenter.Y*_latestFrame.Height)/_drawingCanvas.Height;
                _markerRadius = (_markerRadius*_latestFrame.Height)/_drawingCanvas.Height;
                // Add the marker
                Marker newMarker = _markerDetector.AddMarker("Marker #" + ++_addedMarkerCount, (Bitmap) _latestFrame,
                                                             _markerCenter, _markerRadius);
                comboBoxMarkers.Items.Add(newMarker);

                // Restore the app to its normal state and clear the selection area adorment
                _fAddingMarker = false;
                buttonMarkerAdd.Content = "Add Marker";
                _markerCenter = new Point();
                _drawSelectionAdornment = false;
                //pictureBoxDisplay.Image = new Bitmap(pictureBoxDisplay.Width, pictureBoxDisplay.Height);

                // Enable the demo and marker editing
                comboBoxMarkers.IsEnabled = true;
                comboBoxMarkers.SelectedIndex = comboBoxMarkers.Items.Count - 1;
            }
        }

        private void pictureBoxDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // If the user is selecting a marker, draw a circle of their selection as a selection adornment
            if (_fAddingMarker && !_markerCenter.IsEmpty)
            {
                // Get the current radius
                Point position = e.GetPosition(pictureBoxDisplay).ToDrawingPoint();
                int dx = position.X - _markerCenter.X;
                int dy = position.Y - _markerCenter.Y;
                _markerRadius = (float) Math.Sqrt(dx*dx + dy*dy);
            }
        }
        #endregion

        private void txtThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_markerSelected == null)
                return;

            int value;
            if (int.TryParse(txtThreshold.Text, out value))
            {
                _markerSelected.Threshold = value;
                this.txtThreshold.Background = System.Windows.Media.Brushes.White;
            }
            else
            {
                this.txtThreshold.Background = System.Windows.Media.Brushes.Red;
            }
        }
    }
}