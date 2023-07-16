using System;
using Domain.DTO;
using Domain.Model;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace Domain.UseCase.Interface
{
    public interface IMainFeatureUseCase
    {
        bool IsNotLidarSupport { get; }
        bool IsOverlaySupported { get; }
        bool IsAttackModeBullet { get; }
        IObservable<EyeTrackingSupportCheckerSystem.EyeTrackingState> EyeTrackingChanged { get; }
        
        IObservable<Transform> WinkObservable { get; }
        void SetupWinkChanged();
        GameSettingDto GetGameSetting();
        IObservable<Unit> PostScore(float scoreResult, string playerName);
        
        TrackableId GetTrackableId(string meshFilterName);
        NativeArray<ARMeshClassification> GetClassificationsRawData(TrackableId meshId);
        IObservable<MeshFilter> AddedMeshObservable { get; }
        IObservable<MeshFilter> UpdatedMeshObservable { get; }
        IObservable<MeshFilter> RemovedMeshObservable { get; }

        bool IsClassificationsDisabled(NativeArray<ARMeshClassification> faceClassifications);

        void ReleaseGameObjects();
    }
}