using UnityEngine;
using TMPro;
using View.ViewData;

namespace View
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal sealed class RankText : MonoBehaviour
    {
        public void SetText(RankTextViewData viewData)
        {
           var tmpUGUI = GetComponent<TextMeshProUGUI>();
           tmpUGUI.text = viewData.CellText;
        }
    }
}
