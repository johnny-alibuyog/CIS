namespace CIS.UI.Features;

public interface ITaskScheduler
{
    bool IsWorkInProgress { get; }
    void StartWork();
}
