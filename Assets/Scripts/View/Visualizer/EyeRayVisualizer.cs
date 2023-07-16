using UnityEngine;
using UnityEngine.XR.ARFoundation;
using View.Interface;

namespace View.Visualizer
{
    public sealed class EyeRayVisualizer : IEyeRayVisualizable
    {
        private readonly GameObject _leftEyeGameObject;
        private readonly GameObject _rightEyeGameObject;

        public EyeRayVisualizer(
            GameObject leftEyeGameObject, 
            GameObject rightEyeGameObject)
        {
            this._leftEyeGameObject = leftEyeGameObject;
            this._rightEyeGameObject = rightEyeGameObject;
        }
        public void DisableRay()
        {
            _leftEyeGameObject.gameObject.SetActive(false);
            _rightEyeGameObject.gameObject.SetActive(false);
        }
        
        public void EnableRay()
        {
            _leftEyeGameObject.gameObject.SetActive(true);
            _rightEyeGameObject.gameObject.SetActive(true);
        }

        public void UpdateEyePos(ARFace arFace)
        {
            _leftEyeGameObject.gameObject.transform.position = arFace.leftEye.position; 
            _rightEyeGameObject.gameObject.transform.position = arFace.rightEye.position;
        } 
    } 
}

