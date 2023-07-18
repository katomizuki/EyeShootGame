using System;
using System.Linq;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

namespace Domain.Model
{
    public sealed class WinkTrackingSystem  
    {
        private readonly ARFaceManager _arFaceManager;
        public IObservable<Transform> OnActivateWinkObservable => _onActivatedWinkSubject;
        private readonly Subject<Transform> _onActivatedWinkSubject = new();

        public WinkTrackingSystem(ARFaceManager arFaceManager)
        {
            _arFaceManager = arFaceManager;
        }
        public void SetupFaceChanged()
        {
            _arFaceManager.facesChanged += FaceUpdated; 
        }

        private void FaceUpdated(ARFacesChangedEventArgs args)
        {
            var face = args.updated.FirstOrDefault();
            if (face == null) return;
            var faceSubsystem = (ARKitFaceSubsystem)_arFaceManager.subsystem;
            using (var blendShapes = faceSubsystem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)) {
                foreach (var featureCoefficient in blendShapes) {
                   if (CheckEyeWink(featureCoefficient)) 
                       _onActivatedWinkSubject.OnNext(face.transform);
                }
            }
        }

        private bool CheckEyeWink(ARKitBlendShapeCoefficient featureCoefficient)
        {
            // ウィンク判定
            return (featureCoefficient is 
            { blendShapeLocation: ARKitBlendShapeLocation.EyeBlinkRight
                or ARKitBlendShapeLocation.EyeBlinkRight,
                coefficient: >= 0.8f });
        }
    }
}
