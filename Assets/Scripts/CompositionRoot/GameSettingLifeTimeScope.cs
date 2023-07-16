using DataStore;
using DataStore.Repository;
using DataStore.SciptableObject;
using Domain.UseCase;
using Domain.UseCase.Interface;
using Presenter;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using View;
using View.Interface;

namespace CompositionRoot
{
    internal sealed class GameSettingLifeTimeScope : LifetimeScope 
    {
        [SerializeField] private GameSettingView gameSettingView;
        [SerializeField] private GameSettingScriptableObject gameSettingScriptableObject;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<IGameSettingViewable>(gameSettingView);
            var gameSettingRepository = new GameSettingRepository(gameSettingScriptableObject);
            builder.RegisterInstance<IGameSettingUseCase>(new GameSettingUseCase(gameSettingRepository, new LoadAddressableRepository()));
            builder.RegisterEntryPoint<GameSettingPresenter>();
        }
    } 
}

