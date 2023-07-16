using TMPro;
using UnityEngine;
using View.Interface;

namespace View
{
    public sealed class WarningPanelView : MonoBehaviour, IWarningPanelViewable
    {
        [SerializeField] private TextMeshProUGUI warningEyeTrackingText;
        [SerializeField] private TextMeshProUGUI warningLidarText;
        public void ShowWarningEyeTrackingText(string message)
        {
            gameObject.SetActive(true);
            warningEyeTrackingText.text = message;
        }

        public void ShowWarningLidarText()
        {
            warningLidarText.gameObject.SetActive(true);
            warningLidarText.text = "Lidar is not supported";
        }
        
        public void HiddenWarningPanel()
        {
            gameObject.SetActive(false);
        }
    }
}
