//*****************************************************************************************
//  File:       SnakeDemo.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//
//  Demo to play a simple version of the classic game, snake.
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TouchlessLib;

namespace TouchlessDemo
{
    public class SnakeDemo : IDemoInterface
    {
        public override string ToString() { return "Snake Demo"; }

        #region IDemoInterface

        string IDemoInterface.GetDemoDescription()
        {
            return
@"Snake Demo Instructions:

Uses a single marker to control this classic snake game remake.
>  Use the four areas (up/down/left/right) to direct the snake.
>  The center area doesn't effect the snake direction.
>  The snake won't double-back, turn 90 degrees first.

Collect as many little nibbles as you can to increase your score.
Watch out: the snake will grow and get faster.
Don't hit the walls and don't bite yourself.

Can you make more snake levels with obstacles?
Can you revise the snake's movement and controls?
Can you remake other classic games or invent a new game?
Our SDK isn't tied to Windows Forms, use XNA and more.

Give feedback, submit code, join the community, and more:
http://www.codeplex.com/touchless";
        }

        bool IDemoInterface.StartDemo(TouchlessMgr tlmgr, Rectangle displayBounds)
        {
            if (tlmgr.MarkerCount < 1)
            {
                MessageBox.Show("Snake Demo requires a marker.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _tlmgr = tlmgr;

            _captureWidth = tlmgr.CurrentCamera.CaptureWidth;
            _captureHeight = tlmgr.CurrentCamera.CaptureHeight;
            _displayScale = displayBounds.Width / _captureWidth;
            _canvas = new Bitmap(displayBounds.Width, displayBounds.Height);
            _rows = 24;
            _cols = 32;
            _cellSize = new Size(_captureWidth * _displayScale / _cols, _captureHeight * _displayScale / _rows);
            _backgroundBrush = Brushes.Wheat;
            _collisionBrush = Brushes.Red;
            _nibbleBrush = Brushes.Green;
            _rand = new Random();
            _updatingSnake = false;

            // Make a new snake
            _snake = new Snake(Brushes.Black);
            _snake.body.Insert(0, new Point(_captureWidth * _displayScale / 2, 0));
            _snake.body.Insert(0, new Point(_captureWidth * _displayScale / 2, _cellSize.Height));
            _snake.body.Insert(0, new Point(_captureWidth * _displayScale / 2, _cellSize.Height * 2));
            _snake.dx = _snake.dxm = 0;
            _snake.dy = _snake.dym = 1;
            _lastStateUpdate = DateTime.Now;

            _hudStringMarkerDir = "Null (Center)";
            _hudStringSnakeDir = "Down";
            _hudStringSnakeSize = _snake.body.Count.ToString();

            // Clear the canvas object using the background brush
            Graphics gfx = Graphics.FromImage(_canvas);
            gfx.FillRectangle(_backgroundBrush, 0, 0, _captureWidth * _displayScale, _captureHeight * _displayScale);
            // Draw the initial snake
            Rectangle snakeRect = new Rectangle(_snake.body[2], new Size(_cellSize.Width, 3 * _cellSize.Height));
            gfx.FillRectangle(_snake.brush, snakeRect);
            // Make and draw the first nibble
            _nibble = new Point(_captureWidth * _displayScale / 2, _cellSize.Height * 6);
            Rectangle nibbleRect = new Rectangle(_nibble, _cellSize);
            gfx.FillRectangle(_nibbleBrush, nibbleRect);

            // Create a rectangle that represents the center-origin rect for null input
            _nullZone = new Rectangle(-_captureWidth / 8, -_captureHeight / 8, _captureWidth / 4, _captureHeight / 4);
            _nullZoneDisplay = new Rectangle(_captureWidth / 2 - _captureWidth / 8, _captureHeight / 2 - _captureHeight / 8, _captureWidth / 4, _captureHeight / 4);
            // Precalculate the corner angles for input partitioning
            _angleNW = (int)(Math.Atan2(_captureHeight / 2, -_captureWidth / 2) * 180 / Math.PI);
            _angleNE = (int)(Math.Atan2(_captureHeight / 2, _captureWidth / 2) * 180 / Math.PI);
            _angleSE = (int)(Math.Atan2(-_captureHeight / 2, _captureWidth / 2) * 180 / Math.PI);
            _angleSW = (int)(Math.Atan2(-_captureHeight / 2, -_captureWidth / 2) * 180 / Math.PI);

            // Add marker update handling
            _tlmgr.Markers[0].OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);

            return true;
        }

        void IDemoInterface.StopDemo()
        {
            if (_tlmgr == null || _tlmgr.Markers == null)
                return;

            // Remove marker update handling
            _tlmgr.Markers[0].OnChange += new EventHandler<MarkerEventArgs>(UpdateMarker);

            _tlmgr = null;
        }

        void IDemoInterface.DrawDemoCanvas(Graphics gfx)
        {
            if (_tlmgr == null)
                return;

            // should we Update the snake's state?
            if (!_snake.collision && (DateTime.Now.Ticks - _lastStateUpdate.Ticks) / TimeSpan.TicksPerMillisecond > _snake.updateMS)
            {
                UpdateSnake();
                _lastStateUpdate = DateTime.Now;
            }
            // Draw the game image
            gfx.DrawImage(_canvas, 0, 0);
            // Draw the marker location
            gfx.DrawEllipse(new Pen(Color.Blue, 5), _xm * _displayScale - 10, _ym * _displayScale - 10, 20, 20);
            // Draw the marker direction divisions
            gfx.DrawRectangle(Pens.Blue, _nullZoneDisplay.Left * _displayScale, _nullZoneDisplay.Top * _displayScale, _nullZoneDisplay.Width * _displayScale, _nullZoneDisplay.Height * _displayScale);
            gfx.DrawLine(Pens.Blue, _nullZoneDisplay.Left * _displayScale, _nullZoneDisplay.Top * _displayScale, 0, 0);
            gfx.DrawLine(Pens.Blue, _nullZoneDisplay.Right * _displayScale, _nullZoneDisplay.Top * _displayScale, _captureWidth * _displayScale, 0);
            gfx.DrawLine(Pens.Blue, _nullZoneDisplay.Right * _displayScale, _nullZoneDisplay.Bottom * _displayScale, _captureWidth * _displayScale, _captureHeight * _displayScale);
            gfx.DrawLine(Pens.Blue, _nullZoneDisplay.Left * _displayScale, _nullZoneDisplay.Bottom * _displayScale, 0, _captureHeight * _displayScale);
            // Draw the hud
            gfx.DrawString("Marker Direction:\t" + _hudStringMarkerDir, new Font(FontFamily.GenericSansSerif, 12), Brushes.Brown, new PointF(10, 10));
            gfx.DrawString("Snake Direction:\t" + _hudStringSnakeDir, new Font(FontFamily.GenericSansSerif, 12), Brushes.Brown, new PointF(10, 30));
            gfx.DrawString("Snake Size:\t" + _hudStringSnakeSize, new Font(FontFamily.GenericSansSerif, 12), Brushes.Brown, new PointF(10, 50));
            if (_snake.collision)
                gfx.DrawString("GAME OVER", new Font(FontFamily.GenericSansSerif, 30), Brushes.Brown, new PointF(_captureWidth * _displayScale / 2 - 130, _captureHeight * _displayScale / 2 - 25));
        }

        #endregion IDemoInterface

        private void UpdateSnake()
        {
            if (_updatingSnake)
                return;

            _updatingSnake = true;

            Graphics gfx = Graphics.FromImage(_canvas);

            // Update the actual snake direction with the latest marker direction (prevent double-backing and null setting)
            if ((_snake.dxm != -_snake.dx || _snake.dx == 0) && (_snake.dym != -_snake.dy || _snake.dy == 0) && _snake.dxm != _snake.dym)
            {
                _hudStringSnakeDir = _hudStringMarkerDir;
                _snake.dx = _snake.dxm;
                _snake.dy = _snake.dym;
            }

            // Get the new head point
            Point headPoint = new Point(_snake.body[0].X, _snake.body[0].Y);
            headPoint.X += _cellSize.Width * _snake.dx;
            headPoint.Y += _cellSize.Height * _snake.dy;

            // Check if we've eaten the nibble
            if (!Point.Equals(headPoint, _nibble))
            {
                // Erase the tail piece
                Point tailPoint = _snake.body[_snake.body.Count - 1];
                Rectangle tailRect = new Rectangle(tailPoint, _cellSize);
                gfx.FillRectangle(_backgroundBrush, tailRect);
                _snake.body.RemoveAt(_snake.body.Count - 1);
            }
            else
            {
                // Make a new nibble, not on the snake
                bool nibbleClear = true;
                do
                {
                    _nibble = new Point(_rand.Next(_cols) * _cellSize.Width, _rand.Next(_rows) * _cellSize.Height);
                    Point[] body = new Point[_snake.body.Count];
                    _snake.body.CopyTo(0, body, 0, body.Length);
                    nibbleClear = true;
                    for (int i = 0; i < body.Length; i++)
                        if (Point.Equals(_nibble, body[i]))
                        {
                            nibbleClear = false;
                            break;
                        }
                    _snake.updateMS = (int)(_snake.updateMS * _snake.updateMSMultiplier);
                } while (!nibbleClear);
                Rectangle nibbleRect = new Rectangle(_nibble, _cellSize);
                gfx.FillRectangle(_nibbleBrush, nibbleRect);
            }

            // Check if we've collided with a wall or ourself
            if (headPoint.X < 0 || headPoint.X > _captureWidth * _displayScale - _cellSize.Width ||
               headPoint.Y < 0 || headPoint.Y > _captureHeight * _displayScale- _cellSize.Height)
                _snake.collision = true;
            else
            {
                Point[] body = new Point[_snake.body.Count];
                _snake.body.CopyTo(0, body, 0, body.Length);
                for (int i = 0; i < body.Length; i++)
                    if (Point.Equals(headPoint, body[i]))
                        _snake.collision = true;
            }

            // Add the new head piece
            Rectangle headRect = new Rectangle(headPoint, _cellSize);
            gfx.FillRectangle((_snake.collision) ? _collisionBrush : _snake.brush, headRect);
            _snake.body.Insert(0, headPoint);

            _hudStringSnakeSize = _snake.body.Count.ToString();
            _updatingSnake = false;
        }

        private void UpdateMarker(object sender, MarkerEventArgs args)
        {
            if (!args.EventData.Present)
                return;

            MarkerEventData data = args.EventData;
            _xm = (int)data.X;
            _ym = (int)data.Y;

            // Translate x & y coords to have an origin of the image center, and flip Y
            int x = (int)(data.X - _captureWidth / 2);
            int y = (int)(_captureHeight / 2 - data.Y);

            // Is the marker in our null region?
            if (_nullZone.Contains(new Point(x,y)))
            {
                _hudStringMarkerDir = "Null (Center)";
                _snake.dxm = _snake.dym = 0;
                return;
            }

            // Get the angle of the point from the center
            // angle: E = 0, W = 180(ccw)/-180(cw)
            int angle = (int)(Math.Atan2(y, x) * 180 / Math.PI);

            // Compare the angle with the diagonals and change the snake direction, ignore double-backing
            if (angle < _angleSW || angle >= _angleNW)
            {
                _hudStringMarkerDir = "Left";
                if (_snake.dx == 1)
                    return;
                _snake.dxm = -1;
                _snake.dym = 0;
            }
            else if (angle < _angleSE)
            {
                _hudStringMarkerDir = "Down";
                if (_snake.dy == -1)
                    return;
                _snake.dxm = 0;
                _snake.dym = 1;
            }
            else if (angle < _angleNE)
            {
                _hudStringMarkerDir = "Right";
                if (_snake.dx == -1)
                    return;
                _snake.dxm = 1;
                _snake.dym = 0;
            }
            else /*if (angle < _angleNW)*/
            {
                _hudStringMarkerDir = "Up";
                if (_snake.dy == 1)
                    return;
                _snake.dxm = 0;
                _snake.dym = -1;
            }
        }

        // Drawing and input
        private TouchlessMgr _tlmgr;
        private Bitmap _canvas;
        private int _captureWidth, _captureHeight, _displayScale;
        private string _hudStringMarkerDir, _hudStringSnakeDir, _hudStringSnakeSize;
        private bool _updatingSnake;

        // Input recognition
        private DateTime _lastStateUpdate;
        private Rectangle _nullZone, _nullZoneDisplay;
        private int _angleNW, _angleNE, _angleSE, _angleSW;
        // Marker position
        private int _xm, _ym;

        // Grid info
        private byte _rows, _cols;
        private Size _cellSize;
        private Brush _backgroundBrush;
        private Brush _collisionBrush;
        private Brush _nibbleBrush;

        // Snake info
        private Snake _snake;
        private Random _rand;
        private Point _nibble;

        /// <summary>
        /// Structure to represent the snake
        /// </summary>
        private struct Snake
        {
            public Snake(Brush b)
            {
                dx = dy = dxm = dym = 0;
                body = new List<Point>();
                brush = b;
                collision = false;
                updateMS = 500;
                updateMSMultiplier = 0.97;
            }

            // Snake direction per axis, updated once per step
            public short dx, dy;
            // Marker direction per axis
            public short dxm, dym;
            // Snake Point list that composes its body
            public List<Point> body;
            // The snake brush
            public Brush brush;
            // Has the snake collided?
            public bool collision;
            // The MS to wait between updates, and decrement value
            public int updateMS;
            // The multiplier applied to updateMS for each nibble
            public double updateMSMultiplier;
        }
    }
}
