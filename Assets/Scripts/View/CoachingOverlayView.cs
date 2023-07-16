using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using View.Interface;
using UnityEngine.XR.ARKit;

namespace View 
{
    public sealed class CoachingOverlayView : MonoBehaviour, ICoachingOverlayViewable
    {
        [SerializeField] private ARCoachingGoal goal = ARCoachingGoal.Tracking;
        [SerializeField] private ARSession arSession;
        private ARKitSessionSubsystem _arKitSessionSubsystem;
        private readonly ARKitCustomDelegate _sessionDelegate = new();
        [DllImport("__Internal")]
        private static extern void present_alert(string title, string message);
        private void Awake()
        {
           SetupCoachingOverlay(); 
        }

        private void SetupCoachingOverlay()
        {
            _arKitSessionSubsystem = (ARKitSessionSubsystem) arSession.subsystem;
            _arKitSessionSubsystem.requestedCoachingGoal = goal;
            _arKitSessionSubsystem.coachingActivatesAutomatically = true;
            _arKitSessionSubsystem.sessionDelegate = _sessionDelegate;
        }

        private void OnSessionDidFailWithError(ARKitSessionSubsystem subsystem, NSError error)
        {
            ShowErrorAlert(error);
        }

        private void ShowErrorAlert(NSError error)
        {
            var message = error.localizedDescription;
            var title = "ARSession Failed";
            present_alert(title, message);
        }

        public void ShowOverlay()
        {
            _arKitSessionSubsystem?.SetCoachingActive(true, ARCoachingOverlayTransition.Animated);
            enabled = true;
        }
    
        public void HideOverlay()
        {
            _arKitSessionSubsystem?.SetCoachingActive(false, ARCoachingOverlayTransition.Animated);
            enabled = false;
        }
        
        private void OnEnable()
        {
           _sessionDelegate.OnSessionDidFailWithErrorAction += OnSessionDidFailWithError;
        }

        private void OnDisable()
        {
           _sessionDelegate.OnSessionDidFailWithErrorAction -= OnSessionDidFailWithError;
        }
    }
}