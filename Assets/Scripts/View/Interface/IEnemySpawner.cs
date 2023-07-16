using System;
using UnityEngine;

namespace View.Interface
{
    public interface IEnemySpawner
    {
        IObservable<GameObject> OnTriggerHitObservable { get; }
        void SetUpEnemyTargets(int size, GameObject meteorite);
    }
}