using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;
using View.Interface;
using Object = UnityEngine.Object;

namespace View.Visualizer
{
    public sealed class MeshVisualizer: IMeshVisualizerable
    {
        private readonly MeshFilter _ceilingMeshPrefab;
        private static readonly int NumClassifications = Enum.GetNames(typeof(ARMeshClassification)).Length;
        private readonly Dictionary<TrackableId, MeshFilter[]> _meshHashMap = new();
        private readonly MeshGenerator _meshGenerator = new();

        public MeshVisualizer(MeshFilter meshFilter)
        {
           _ceilingMeshPrefab = meshFilter; 
        }
        
      
        public void RemoveMeshFilters(TrackableId meshId)
        {
            var meshFilters = _meshHashMap[meshId];
            for (int i = 0; i < NumClassifications; ++i)
            {
                var classifiedMeshFilter = meshFilters[i];
                if (classifiedMeshFilter != null)
                    Object.Destroy(classifiedMeshFilter);
            }
            _meshHashMap.Remove(meshId);
        }

        public void AddMeshFilters(NativeArray<ARMeshClassification> faceClassifications, MeshFilter meshFilter, TrackableId meshId)
        {
            using (faceClassifications)
            {
                MeshFilter[] meshFilters = new MeshFilter[NumClassifications];
                meshFilters[(int)ARMeshClassification.Ceiling] = Object.Instantiate(_ceilingMeshPrefab, meshFilter.transform.parent);
                _meshHashMap[meshId] = meshFilters;
                UpdateClassifiedMesh(faceClassifications, meshFilters, meshFilter.sharedMesh);
            }
        }

        public void UpdateMeshFilters(NativeArray<ARMeshClassification> faceClassifications, MeshFilter meshFilter, TrackableId meshId)
        {
            using (faceClassifications)
            {
                UpdateClassifiedMesh(faceClassifications, _meshHashMap[meshId], meshFilter.sharedMesh);
            }
        }

        private void UpdateClassifiedMesh(NativeArray<ARMeshClassification> faceClassifications, MeshFilter[] meshFilters, Mesh baseMesh)
        {
            for (int i = 0; i < NumClassifications; ++i)
            {
                var classifiedMeshFilter = meshFilters[i];
                if (classifiedMeshFilter != null)
                    meshFilters[i].mesh = _meshGenerator.MakeClassifiedMesh(
                        baseMesh, 
                        faceClassifications, 
                        (ARMeshClassification)i,
                        classifiedMeshFilter.mesh);
            } 
        }
    }
}