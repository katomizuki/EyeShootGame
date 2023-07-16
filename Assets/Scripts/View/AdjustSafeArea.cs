using UnityEngine;

namespace View
{
    [RequireComponent(typeof(RectTransform))]
    internal sealed class AdjustSafeArea : MonoBehaviour
    {
        private void Start()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMax.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}
