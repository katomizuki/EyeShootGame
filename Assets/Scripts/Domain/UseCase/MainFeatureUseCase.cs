using System;
using Domain.DTO;
using Domain.IRepository;
using Domain.Model;
using Domain.UseCase.Interface;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

namespace Domain.UseCase
{
    public sealed class MainFeatureUseCase: IMainFeatureUseCase
    {
        private readonly EyeTrackingSupportCheckerSystem _eyeTrackingSupportCheckerSystem;
        private readonly LidarSupportCheckerSystem _lidarSupportCheckerSystem; 
        private readonly OverlaySupportCheckerSystem _overlaySupportCheckerSystem;
        private readonly IGameSettingRepository _gameSettingRepository;
        private readonly WinkTrackingSystem _winkTrackingSystem;
        private readonly IScoreResultRepository _scoreResultRepository;
        private readonly ClassficationMeshSystem _classficationMeshSystem;
        private readonly ILoadAddressableRepository _loadAddressableRepository;

        public MainFeatureUseCase(
            EyeTrackingSupportCheckerSystem eyeTrackingSupportCheckerSystem,
            LidarSupportCheckerSystem lidarSupportCheckerSystem,
            OverlaySupportCheckerSystem overlaySupportCheckerSystem,
            IGameSettingRepository gameSettingRepository,
            IScoreResultRepository scoreResultRepository,
            WinkTrackingSystem winkTrackingSystem,
            ClassficationMeshSystem classficationMeshSystem,
            ILoadAddressableRepository loadAddressableRepository
            )
        {
            this._eyeTrackingSupportCheckerSystem = eyeTrackingSupportCheckerSystem;
            this._lidarSupportCheckerSystem = lidarSupportCheckerSystem;
            this._overlaySupportCheckerSystem = overlaySupportCheckerSystem;
            this._gameSettingRepository = gameSettingRepository;
            this._scoreResultRepository = scoreResultRepository;
            this._winkTrackingSystem = winkTrackingSystem;
            this._classficationMeshSystem = classficationMeshSystem;
            this._loadAddressableRepository = loadAddressableRepository;
        }
        public bool IsNotLidarSupport => !_lidarSupportCheckerSystem.IsLidarSupport;
        public IObservable<EyeTrackingSupportCheckerSystem.EyeTrackingState> EyeTrackingChanged
       => _eyeTrackingSupportCheckerSystem.OnEyeTrackingStateChanged;
        public bool IsAttackModeBullet => _gameSettingRepository.GetGameSetting()
            .GameAttackMode == GameAttackMode.Bullet;
        public bool IsOverlaySupported => _overlaySupportCheckerSystem.IsOverlaySupported;
        public void SetupWinkChanged() => _winkTrackingSystem.SetupFaceChanged();
        public IObservable<Transform> WinkObservable => _winkTrackingSystem.OnActivateWinkObservable;
        public GameSettingDto GetGameSetting() => _gameSettingRepository.GetGameSetting();
        public IObservable<Unit> PostScore(float scoreResult, string playerName)
        {
            return Observable.FromCoroutine<Unit>(
                observer =>
                    _scoreResultRepository.PostResult(
                        observer,
                        scoreResult,
                        playerName));
        }

        public bool IsClassificationsDisabled(NativeArray<ARMeshClassification> faceClassifications) =>
            (!faceClassifications.IsCreated || faceClassifications.Length <= 0);

        public TrackableId GetTrackableId(string meshFilterName) => _classficationMeshSystem.MakeTrackableId(meshFilterName);
        public NativeArray<ARMeshClassification> GetClassificationsRawData(TrackableId meshId) => _classficationMeshSystem.MakeClassificationsRawData(meshId);
        public IObservable<MeshFilter> AddedMeshObservable => _classficationMeshSystem.AddedMeshObservable;
        public IObservable<MeshFilter> UpdatedMeshObservable => _classficationMeshSystem.UpdatedMeshObservable;
        public IObservable<MeshFilter> RemovedMeshObservable => _classficationMeshSystem.RemovedMeshObservable;

        public void ReleaseGameObjects()
        {
            _loadAddressableRepository.ReleaseAsteroid();
            var starBullet = _gameSettingRepository.GetGameSetting().StarBullet;
            _loadAddressableRepository.ReleaseStar(starBullet);
        }
    }
}