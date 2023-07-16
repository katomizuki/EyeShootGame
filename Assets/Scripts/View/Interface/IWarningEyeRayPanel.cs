namespace View.Interface
{
    public interface IWarningPanelViewable
    {
        void ShowWarningEyeTrackingText(string message);
        void ShowWarningLidarText();
        void HiddenWarningPanel();
    }
}