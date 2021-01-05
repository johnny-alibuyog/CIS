using System.Collections.Generic;
using System.ServiceModel;
using Touchless.Vision.Contracts;

namespace Touchless.Vision.Service.Host
{
    public interface ITouchlessClient
    {
        [OperationContract(IsOneWay = true)]
        void OnObjectsDetected(Frame frame, IEnumerable<DetectedObject> detectedObjects);

        [OperationContract(IsOneWay = true)]
        void OnNewObject(Frame frame, DetectedObject detectedObject);

        [OperationContract(IsOneWay = true)]
        void OnObjectMoved(Frame frame, DetectedObject detectedObject);

        [OperationContract(IsOneWay = true)]
        void OnObjectRemoved(Frame frame, DetectedObject detectedObject);
    }
}