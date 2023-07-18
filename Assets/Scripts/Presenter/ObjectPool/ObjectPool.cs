using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Presenter.ObjectPool 
{
    public sealed class ObjectPool : IObjectPool
    {
        private readonly uint _poolSize;
        private readonly GameObject _targetPrefab;
        private Stack<GameObject> _stack;

        public ObjectPool(GameObject targetPrefab, uint poolSize)
        {
            this._targetPrefab = targetPrefab;
            this._poolSize = poolSize;
        }

        public void SetupPool()
        {
            _stack = new Stack<GameObject>();
            for (int i = 0; i < _poolSize; i++)
            {
                var poolTarget = Object.Instantiate(_targetPrefab);
                poolTarget.SetActive(false);
                _stack.Push(poolTarget);
            }
        }

        public void OnRelease(PoolTarget poolTarget)
        {
            poolTarget.gameObject.SetActive(false);
            poolTarget.OnRelease -= OnRelease;
            _stack.Push(poolTarget.gameObject);
        }

        public GameObject GetPooledObject()
        {
            Assert.IsNotNull(_stack);
            
            if (_stack.Count == 0)
            {
                var newPooledObject = Object.Instantiate(_targetPrefab);
                newPooledObject.SetActive(true);
                newPooledObject.GetComponent<PoolTarget>().OnRelease += OnRelease;
                return newPooledObject;
            }

            var nextPooledObject = _stack.Pop();
            nextPooledObject.SetActive(true);
            nextPooledObject.GetComponent<PoolTarget>().OnRelease += OnRelease;
            return nextPooledObject;
        } 
    }
}