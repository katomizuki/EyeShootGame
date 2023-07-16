using System;
using UnityEngine;

namespace View 
{
    internal sealed class ClashByTriggerWeapons : MonoBehaviour
    {
        public Action<GameObject> OnTriggerHit;
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerHit?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
