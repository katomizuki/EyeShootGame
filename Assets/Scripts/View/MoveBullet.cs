using UnityEngine;

namespace View 
{
    internal sealed class MoveBullet : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f)] private float speed;
        private Camera _cachedCamera;
        private void OnEnable()
        {
            _cachedCamera = Camera.main;
            // 画面中央に移動。
            transform.position =  _cachedCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _cachedCamera.nearClipPlane));
            // Cameraの向きに合わせて回転。
            transform.rotation = _cachedCamera.transform.rotation * Quaternion.Euler(-90f, 0f, 0f);
        }

        private void Update()
        {
            transform.position += _cachedCamera.transform.forward * speed * Time.deltaTime;
        }
    }
}