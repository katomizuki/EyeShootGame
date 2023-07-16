using System;
using System.Collections.Generic;
using Domain.Model;
using Domain.UseCase.Interface;
using Presenter.ObjectPool;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using View.Interface;

namespace Presenter
{
    public sealed class MainFeaturePresenter : IDisposable, IPostInitializable, IStartable 
    {
        private readonly CompositeDisposable _disposable = new();
        // View
        private readonly IWarningPanelViewable _warningEyeRayPanelViewable;
        private readonly ICoachingOverlayViewable _coachingOverlayViewable;
        private readonly IMeshVisualizerable _meshVisualizerable;
        private readonly IEyeRayVisualizable _eyeRayVisualizer;
        private readonly IScoreViewable _scoreView;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IObjectPool _heartObjectPool;
        private readonly Dictionary<int, GameObject> _hashMap = new();
        private int _currentDestroyedMeteorites;
        // UseCase
        private readonly IMainFeatureUseCase _useCase;
       

        [Inject]
        MainFeaturePresenter(
            IWarningPanelViewable warningEyeRayPanelViewable,
            ICoachingOverlayViewable coachingOverlayViewable,
            IMeshVisualizerable meshVisualizerable,
            IEyeRayVisualizable eyeRayVisualizer,
            IScoreViewable scoreView,
            IEnemySpawner enemySpawner,
            IObjectPool heartObjectPool,
            IMainFeatureUseCase useCase)
        {
            this._warningEyeRayPanelViewable = warningEyeRayPanelViewable;
            this._coachingOverlayViewable = coachingOverlayViewable;
            this._meshVisualizerable = meshVisualizerable;
            this._eyeRayVisualizer = eyeRayVisualizer;
            this._scoreView = scoreView;
            this._enemySpawner = enemySpawner;
            this._heartObjectPool = heartObjectPool;
            this._currentDestroyedMeteorites = 0;
            this._useCase = useCase;
        }

        public void PostInitialize()
        {
            SetupBullet();
            SetUpGameSettingInformation();

            // Handle View Visibility
            HandleLidarWarningVisibility(); 
            HandleEyeRayVisibility(); 
            
            // Subscribe
            SubscribeEyeTrackingState();
            SubscribeWinkTracking();
            SubscribeAddedMesh();
            SubscribeUpdateMesh();
            SubscribeRemovedMesh();
            SubscribeHitMeteorite();
            SubscribeTimeUp(); 
            SubscribeGameClear();
            SubscribeCloseButton();
        }
        
        public void Start()
        {
            // Handle View Visibility
            HandleOverlayVisibility();
        }

        private void SubscribeEyeTrackingState()
        {
            _useCase.EyeTrackingChanged
                .Subscribe(state =>
                {
                    CheckSupportEyeTracking(state); 
                })
                .AddTo(_disposable); 
        }
        
        private void HandleOverlayVisibility()
        {
            if (_useCase.IsOverlaySupported)
                _coachingOverlayViewable.ShowOverlay();
            else
                _coachingOverlayViewable.HideOverlay();  
        }
        
        private void HandleEyeRayVisibility()
        {
            if (_useCase.IsAttackModeBullet)
                _eyeRayVisualizer.DisableRay();
            else 
                _eyeRayVisualizer.EnableRay();
        }
        

        private void HandleLidarWarningVisibility()
        {
            if (_useCase.IsNotLidarSupport)
                _warningEyeRayPanelViewable.ShowWarningLidarText(); 
        }
      
        private void CheckSupportEyeTracking(EyeTrackingSupportCheckerSystem.EyeTrackingState state)
        {
            switch (state)
            {
                case EyeTrackingSupportCheckerSystem.EyeTrackingState.Ready:
                    _warningEyeRayPanelViewable.HiddenWarningPanel();
                    break;
                case EyeTrackingSupportCheckerSystem.EyeTrackingState.NotReady:
                    _warningEyeRayPanelViewable.ShowWarningEyeTrackingText("Eye Tracking is not ready");
                    break;
                case EyeTrackingSupportCheckerSystem.EyeTrackingState.NotSupport:
                    _warningEyeRayPanelViewable.ShowWarningEyeTrackingText("Eye Tracking is not supported");
                    break;
                case EyeTrackingSupportCheckerSystem.EyeTrackingState.UnKnown:
                    _warningEyeRayPanelViewable.ShowWarningEyeTrackingText("Unknown Error");
                    break;
            }
        }
        
        private void SubscribeWinkTracking()
        {
            _useCase.WinkObservable
                .ThrottleFirst(TimeSpan.FromMilliseconds(100)) //異常な連射を防止
                .Subscribe(_ =>
                {
                    ShowBullet();
                })
                .AddTo(_disposable); 
        }
        
        private void SetupBullet()
        {
            if (_useCase.IsAttackModeBullet)
            {
                _useCase.SetupWinkChanged();
                _heartObjectPool.SetupPool(); 
            }
        }
        
        private void SubscribeAddedMesh()
        {
            _useCase.AddedMeshObservable
                .Subscribe(meshFilter =>
                {
                    var meshId = _useCase.GetTrackableId(meshFilter.name);
                    var faceClassifications = _useCase.GetClassificationsRawData(meshId);
                    if (_useCase.IsClassificationsDisabled(faceClassifications))
                        return;
                    _meshVisualizerable.AddMeshFilters(faceClassifications, meshFilter, meshId);
                })
                .AddTo(_disposable); 
        }

        private void SubscribeUpdateMesh()
        {
            _useCase.UpdatedMeshObservable
                .Subscribe(meshFilter =>
                {
                    var meshId = _useCase.GetTrackableId(meshFilter.name);
                    var faceClassifications = _useCase.GetClassificationsRawData(meshId); 
                    if (_useCase.IsClassificationsDisabled(faceClassifications))
                        return;
                    _meshVisualizerable.UpdateMeshFilters(faceClassifications, meshFilter, meshId);
                })
                .AddTo(_disposable); 
        }
        
        private void SubscribeRemovedMesh()
        {
            _useCase.RemovedMeshObservable
                .Subscribe(mesh =>
                {
                    var meshId = _useCase.GetTrackableId(mesh.name);
                    _meshVisualizerable.RemoveMeshFilters(meshId);
                })
                .AddTo(_disposable); 
        }
        
          private void SetUpGameSettingInformation()
        {
            var gameSetting = _useCase.GetGameSetting();
            _scoreView.SetScoreText($"0/{gameSetting.NumberOfEnemies.ToString()}");
            _scoreView.SetCountDownTimeText(gameSetting.GameTimeLimit);
            _enemySpawner.SetUpEnemyTargets(gameSetting.NumberOfEnemies, gameSetting.Meteorite); 
        }
        
        private void SubscribeHitMeteorite()
        {
            _enemySpawner.OnTriggerHitObservable
                .Subscribe(go =>
                {
                    var id = go.GetInstanceID();
                    if (_hashMap.ContainsKey(id))
                        return;
                    // 破壊したObjectを入れる。
                    _hashMap[id] = go;
                    _currentDestroyedMeteorites++;
                    var gameSetting = _useCase.GetGameSetting();
                    _scoreView.SetScoreText($"{_currentDestroyedMeteorites}/{gameSetting.NumberOfEnemies.ToString()}");
                    if (_currentDestroyedMeteorites == gameSetting.NumberOfEnemies)
                        _scoreView.OnGameClearAction();
                })
                .AddTo(_disposable); 
        }

        private void SubscribeTimeUp()
        {
            _scoreView.OnTimeUp
                .Subscribe(_ =>
                {
                    _scoreView.StopTimer();
                    _scoreView.ShowIndicator();
                    var gameSetting = _useCase.GetGameSetting();
                    var playerName = gameSetting.PlayerName;
                    _useCase.PostScore(0, playerName)
                        .Subscribe(
                            _ =>
                            {
                                _useCase.ReleaseGameObjects();
                                _scoreView.ShowTimeUpAlert(); 
                                _scoreView.HideIndicator();
                            },
                            _ => _scoreView.ShowErrorAlert())
                        .AddTo(_disposable);
                })
                .AddTo(_disposable); 
        }

        private void SubscribeGameClear()
        {
            _scoreView.OnGameClear
                .Subscribe(timeLeft =>
                {
                    _scoreView.StopTimer();
                    _scoreView.ShowIndicator();
                    var gameSetting = _useCase.GetGameSetting();
                    var consumeptionTime = gameSetting.GameTimeLimit - timeLeft;
                    var scoreResult = (float)Math.Round(gameSetting.NumberOfEnemies / consumeptionTime, 2, MidpointRounding.AwayFromZero);;
                    
                    _useCase.PostScore(scoreResult, gameSetting.PlayerName)
                        .Subscribe(
                            _ =>
                            {
                                _useCase.ReleaseGameObjects();
                                _scoreView.HideIndicator();
                                _scoreView.ShowResultScene(scoreResult);
                            }, 
                            _ => _scoreView.ShowErrorAlert())
                        .AddTo(_disposable);
                })
                .AddTo(_disposable); 
        }

        private void SubscribeCloseButton()
        {
            _scoreView.OnTapCloseButton
                .Subscribe(_ =>
                {
                    _scoreView.MoveToEndScreen();
                })
                .AddTo(_disposable); 
        }
        
        private void ShowBullet()
        {
            var bullet = _heartObjectPool.GetPooledObject();
            bullet.SetActive(true);
            bullet.GetComponent<PoolTarget>().Deactivate();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}