using UnityEngine;

namespace Presenter.ObjectPool
{
    public interface IObjectPool
    {
        void SetupPool();
        GameObject GetPooledObject();
        void OnRelease(PoolTarget poolTarget);
    }
}