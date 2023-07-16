using System;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace View.Interface
{
    public interface IMeshVisualizerable
    {
        void RemoveMeshFilters(TrackableId meshId);
        void  AddMeshFilters(
            NativeArray<ARMeshClassification> faceClassifications, 
            MeshFilter meshFilter, 
            TrackableId meshId);

        void UpdateMeshFilters(
            NativeArray<ARMeshClassification> faceClassifications, 
            MeshFilter meshFilter,
            TrackableId meshId);
    }
}