using UnityEngine.XR.ARKit;

namespace Domain.Model
{
    public class OverlaySupportCheckerSystem 
    {
       public bool IsOverlaySupported => ARKitSessionSubsystem.coachingOverlaySupported;
    }
}