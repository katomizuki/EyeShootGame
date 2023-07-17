using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

namespace Domain.Model
{
    public sealed class LidarSupportCheckerSystem 
    {
        public bool IsLidarSupport {
            get
            {
                var loader = LoaderUtility.GetActiveLoader();
                var isSupport = loader && loader.GetLoadedSubsystem<XRMeshSubsystem>() != null;
                return isSupport;
            }
        }
    }
}