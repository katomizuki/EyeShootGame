using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace View
{
    [RequireComponent(typeof(ARFace))]
   internal sealed class MoveEyeLazer: MonoBehaviour
    {
        [SerializeField] private GameObject leftEyeGameObject;
        [SerializeField] private GameObject rightEyeGameObject;
        private ARFace _arFace;
        private void Awake()
        {
            _arFace = GetComponent<ARFace>();
            
        }

        private void OnEnable()
        {
            _arFace.updated += FaceUpdated;  
        }

        private void FaceUpdated(ARFaceUpdatedEventArgs args)
        {
            var arFace = args.face;
            leftEyeGameObject.gameObject.transform.position = arFace.leftEye.position; 
            rightEyeGameObject.gameObject.transform.position = arFace.rightEye.position;
        }

        private void OnDisable()
        {
            _arFace.updated -= FaceUpdated;
        }
    }
}