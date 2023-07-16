using System;
using System.Collections;
using UnityEngine;

namespace Presenter.ObjectPool 
{
    public sealed class PoolTarget : MonoBehaviour
    {
        public Action<PoolTarget> OnRelease;
        private readonly WaitForSeconds _cacheWaitForSeconds = new(0.5f);

        private IEnumerator Release()
        {
            yield return _cacheWaitForSeconds;
            OnRelease?.Invoke(this);
            gameObject.SetActive(false);
        }
        
        public void Deactivate()
        {
            StartCoroutine(Release());
        }
    }
}