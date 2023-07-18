using System;
using UniRx;
using UnityEngine.XR.ARFoundation;

namespace Domain.Model
{
    public sealed class EyeTrackingSupportCheckerSystem 
    {
        
        public enum EyeTrackingState
        {
            Ready,
            NotReady,
            NotSupport,
            UnKnown
        }
        private readonly ARFaceManager _arFaceManager;
        private readonly Subject<EyeTrackingState> _onArSessionStateChangedSubject = new();
        public IObservable<EyeTrackingState> OnEyeTrackingStateChanged => _onArSessionStateChangedSubject;
        
        public EyeTrackingSupportCheckerSystem(ARFaceManager arFaceManager)
        {
            this._arFaceManager = arFaceManager;
            ARSession.stateChanged += OnStateChanged;
        }

        private EyeTrackingState GetEyeTracking(ARSessionState state)
        {
            return (IsArSessionReady(state), IsSupportEyeTracking) switch 
            {
                (true, true) => EyeTrackingState.Ready,
                (true, false) => EyeTrackingState.NotSupport,
                (false, true) => EyeTrackingState.NotReady,
                _ => EyeTrackingState.UnKnown
            };
        }

        private void OnStateChanged(ARSessionStateChangedEventArgs args)
        {
            _onArSessionStateChangedSubject.OnNext(GetEyeTracking(args.state));
        }

        private bool IsSupportEyeTracking => _arFaceManager.descriptor.supportsEyeTracking;
        private bool IsArSessionReady(ARSessionState state) => ARSessionState.Ready < state;
    }
}
