using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Touchless.Shared.Extensions;
using System.Drawing;
using System.Diagnostics;
using Touchless.Vision.Contracts;

namespace Touchless.Vision.Tracking
{
    public class ObjectTrackingService
    {
        public event Action<ObjectTrackingService, DetectedObject, Frame> NewObject, ObjectMoved, ObjectRemoved;

        private Dictionary<int, DetectedObjectSnapshot> _detectedObjects;

        public ObjectTrackingService()
        {
            _detectedObjects = new Dictionary<int, DetectedObjectSnapshot>();
        }

        public void UpdateDetectedObjects(Frame frame, IEnumerable<DetectedObject> detectedObjects)
        {
            foreach (DetectedObject detectedObject in detectedObjects)
            {
                DetectedObjectSnapshot snapshot;

                if (!_detectedObjects.ContainsKey(detectedObject.Id))
                {
                    snapshot = new DetectedObjectSnapshot
                    {
                        Position = detectedObject.Position,
                        DetectedObject = detectedObject
                    };
                    _detectedObjects.Add(detectedObject.Id, snapshot);

                    Debug.WriteLine("********NEW MARKER*********");
                    this.NewObject.IfNotNull(i => i(this, detectedObject, frame));
                }
                else
                {
                    snapshot = _detectedObjects[detectedObject.Id];

                    if (snapshot.Position != detectedObject.Position)
                    {
                        snapshot.Position = detectedObject.Position;
                        Debug.WriteLine("********MARKER MOVED*********");
                        this.ObjectMoved.IfNotNull(i => i(this, detectedObject, frame));
                    }
                }

               
            }

            if (this.ObjectRemoved != null)
            {
                Debug.WriteLine("********MARKER REMOVED*********");
                _detectedObjects.Keys
                    .Where(i => !detectedObjects.Any(j => j.Id == i))
                    .ForEach(i => this.ObjectRemoved(this, _detectedObjects[i].DetectedObject, frame));
            }
        }

        private struct DetectedObjectSnapshot
        {
            public DetectedObject DetectedObject { get; set; }
            public Point Position { get; set; }
        }
    }
}
