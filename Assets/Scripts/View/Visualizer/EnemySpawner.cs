using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using View.Interface;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace View.Visualizer
{
    public sealed class EnemySpawner : IEnemySpawner, IDisposable
    {
        private readonly ParticleSystem _particleSystem;
        private readonly GameObject _parentGameObject;
        private readonly float _radius;
        private readonly List<ClashByTriggerWeapons> _eyeRayTriggers = new(); 
        private readonly Subject<GameObject> _onTriggerHitSubject = new();
        public IObservable<GameObject> OnTriggerHitObservable => _onTriggerHitSubject;
        
        public EnemySpawner(
            ParticleSystem particleSystem, 
            float radius, 
            GameObject parentGameObject)
        {
            this._particleSystem = particleSystem;
            this._radius = radius;
            this._parentGameObject = parentGameObject;
        }
        
        public void SetUpEnemyTargets(int size, GameObject meteorite)
        {
            var playerTransform = Camera.main!.transform;
            for (int i = 0; i < size; i++)
            {
                var randomDirection = Random.insideUnitSphere.normalized;
                var randomPosition = randomDirection * _radius + playerTransform.position;
                var meteoriteGameObject = Object.Instantiate(meteorite, randomPosition, Quaternion.identity);
               meteoriteGameObject.transform.parent = _parentGameObject.transform;
                var eyeRayTrigger = meteoriteGameObject.GetComponent<ClashByTriggerWeapons>();
                eyeRayTrigger.OnTriggerHit += OnTriggerHit;
                _eyeRayTriggers.Add(eyeRayTrigger);
            } 
        } 

        private void OnTriggerHit(GameObject go)
        {
            Object.Instantiate(_particleSystem, go.transform.position, Quaternion.identity);
            _particleSystem.Play();
            _onTriggerHitSubject.OnNext(go);
        }

        public void Dispose()
        {
            _onTriggerHitSubject?.Dispose();
            foreach (var eyeRayTrigger in _eyeRayTriggers)
            {
                eyeRayTrigger.OnTriggerHit -= OnTriggerHit;
            } 
        }
    }
}
