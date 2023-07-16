using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using View.Interface;
using View.ViewData;

namespace View
{
    public sealed class RankingScrollView : MonoBehaviour, IRankingScrollViewable
    {
        private RankText[] _rankTexts;
        [DllImport("__Internal")]
        private static extern void present_alert(string title, string message);
        [DllImport("__Internal")]
        private static extern void present_indicator();
        [DllImport("__Internal")]
        private static extern void hide_indicator();
        private void Awake()
        {
            _rankTexts = GetComponentsInChildren<RankText>();
        }

        public void ShowScoreResults(List<RankTextViewData> viewDatas)
        {
            for (int i = 0; i < viewDatas.Count; i++)
            {
                if (_rankTexts[i] != null)
                  _rankTexts[i].SetText(viewDatas[i]);
            }
        }

        public void ShowIndicator()
        {
            present_indicator();
        }

        public void HideIndicator()
        {
            hide_indicator();
        }

        public void ShowErrorAlert()
        {
            present_alert("Error", "Failed to get score results.");
        }
    }
}