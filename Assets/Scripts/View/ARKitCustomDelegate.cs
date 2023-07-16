using System;
using UnityEngine.XR.ARKit;

namespace View
{
    internal sealed class ARKitCustomDelegate: DefaultARKitSessionDelegate
    {
        public Action<ARKitSessionSubsystem, NSError> OnSessionDidFailWithErrorAction;
    
        protected override void OnSessionDidFailWithError(ARKitSessionSubsystem sessionSubsystem, NSError error)
        {
            OnSessionDidFailWithErrorAction?.Invoke(sessionSubsystem, error);
        }
    }
}