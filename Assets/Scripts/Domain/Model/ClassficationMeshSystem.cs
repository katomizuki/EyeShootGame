using System;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace Domain.Model
{
    public sealed class ClassficationMeshSystem : IDisposable 
    {
        private readonly ARMeshManager _arMeshManager;
        private readonly Subject<MeshFilter> _addedMeshSubject = new();
        private readonly Subject<MeshFilter> _updatedMeshSubject = new();
        private readonly Subject<MeshFilter> _removedMeshSubject = new();
        public IObservable<MeshFilter> AddedMeshObservable => _addedMeshSubject;
        public IObservable<MeshFilter> UpdatedMeshObservable => _updatedMeshSubject;
        public IObservable<MeshFilter> RemovedMeshObservable => _removedMeshSubject;
        
        public ClassficationMeshSystem(ARMeshManager arMeshManager)
        {
            this._arMeshManager = arMeshManager;
            SetupMeshStateChanged();
            SetUpClassfication();
        }

        private void SetupMeshStateChanged()
        {
            _arMeshManager.meshesChanged += OnMeshChanged;
        }

        private void OnMeshChanged(ARMeshesChangedEventArgs args)
        {
            args.added?.ForEach(AddedMesh);
            args.updated?.ForEach(UpdatedMesh);
            args.removed?.ForEach(RemovedMesh);
        }

        private void AddedMesh(MeshFilter meshFilter)
        {
            _addedMeshSubject.OnNext(meshFilter);
        }
        
        private void UpdatedMesh(MeshFilter meshFilter)
        {
            _updatedMeshSubject.OnNext(meshFilter);
        }
        
        private void RemovedMesh(MeshFilter meshFilter)
        {
            _removedMeshSubject.OnNext(meshFilter);
        }

        private bool IsClassificationEnabled
            => (_arMeshManager != null) && (_arMeshManager.subsystem is XRMeshSubsystem meshSubsystem);

        public TrackableId MakeTrackableId(string meshFilterName)
        {
            string[] nameSplit = meshFilterName.Split(' ');
            return new TrackableId(nameSplit[1]);
        }

        public NativeArray<ARMeshClassification> MakeClassificationsRawData(TrackableId meshId)
        {
            XRMeshSubsystem meshSubsystem = _arMeshManager.subsystem;
            var faceClassifications = meshSubsystem.GetFaceClassifications(meshId, Allocator.Persistent);
            return faceClassifications;
        }
        
        private void SetUpClassfication()
        {
            if (IsClassificationEnabled)
                _arMeshManager.subsystem.SetClassificationEnabled(true);
        }
        
        public void Dispose()
        {
            _arMeshManager.meshesChanged -= OnMeshChanged;
        }
    }
}

