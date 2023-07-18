using DataStore.Repository;
using DataStore.SciptableObject;
using Domain.Model;
using Domain.UseCase;
using Domain.UseCase.Interface;
using Presenter;
using Presenter.ObjectPool;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;
using VContainer.Unity;
using View;
using View.Interface;
using View.Visualizer;

namespace CompositionRoot
{
    internal sealed class MainFeatureLifeTimeScope : LifetimeScope
    {
        [SerializeField] private ARFaceManager arFaceManager;
        [SerializeField] private ARMeshManager arMeshManger;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private CoachingOverlayView coachingOverlayView;
        [SerializeField] private WarningPanelView warningPanelView;
        [SerializeField] private GameSettingScriptableObject gameSettingScriptableObject;
        [SerializeField,Range(10, 20)] private uint poolSize;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private ParticleSystem meteoriteExplosion;
        [SerializeField, Range(0.5f, 2.5f)] private float radius;
        [SerializeField] private GameObject parentGameObject;

        protected override void Configure(IContainerBuilder builder)
        {
            var eyeTrackingSupport = new EyeTrackingSupportCheckerSystem(arFaceManager);
            var lidarSupport = new LidarSupportCheckerSystem();
            var overlaySupportCheckerSystem = new OverlaySupportCheckerSystem();
            var gameSettingRepository = new GameSettingRepository(gameSettingScriptableObject);
            var resultRepository = new ScoreResultRepository();
            var facePrefab = arFaceManager.facePrefab;
            var leftEyeGameObject = facePrefab.transform.GetChild(0).gameObject;
            var rightEyeGameObject = facePrefab.transform.GetChild(1).gameObject;
            var meshSystem = new ClassficationMeshSystem(arMeshManger);
            var winkTrackingSystem = new WinkTrackingSystem(arFaceManager);
            var loadAddressableRepository = new LoadAddressableRepository();
           
            var useCase = new MainFeatureUseCase(
                eyeTrackingSupport,
                lidarSupport,
                overlaySupportCheckerSystem,
                gameSettingRepository,
                resultRepository,
                winkTrackingSystem,
                meshSystem, 
                loadAddressableRepository);
            
            var eyeRayVisualizer = new EyeRayVisualizer(leftEyeGameObject, rightEyeGameObject);
            var meshVisualizer = new MeshVisualizer(meshFilter);
            var enemySpawner = new EnemySpawner(meteoriteExplosion, radius, parentGameObject);
            
            builder.RegisterInstance<IMainFeatureUseCase>(useCase); 
            builder.RegisterInstance<IEyeRayVisualizable>(eyeRayVisualizer);
            builder.RegisterInstance<IMeshVisualizerable>(meshVisualizer);
            builder.RegisterInstance<IEnemySpawner>(enemySpawner);
             
            builder.RegisterComponent<IObjectPool>(new ObjectPool(gameSettingScriptableObject.Bullet, poolSize));
            builder.RegisterComponent<IScoreViewable>(scoreView);
            builder.RegisterComponent<ICoachingOverlayViewable>(coachingOverlayView);
            builder.RegisterComponent<IWarningPanelViewable>(warningPanelView);
            
            builder.RegisterEntryPoint<MainFeaturePresenter>();
        }
    }
}