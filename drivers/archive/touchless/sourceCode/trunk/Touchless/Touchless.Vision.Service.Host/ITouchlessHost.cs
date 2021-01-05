using System.ServiceModel;

namespace Touchless.Vision.Service.Host
{
    [ServiceContract(CallbackContract = typeof (ITouchlessClient))]
    public interface ITouchlessHost
    {
        [OperationContract]
        bool RegisterClient();

        [OperationContract]
        bool UnRegisterClient();

        [OperationContract]
        void StartFrameProcessing();

        [OperationContract]
        void StopFrameProcessing();
    }
}