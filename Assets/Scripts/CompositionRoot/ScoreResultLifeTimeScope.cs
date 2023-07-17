using DataStore.Repository;
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
    internal sealed class ScoreResultLifeTimeScope : LifetimeScope 
    {
        [SerializeField] private RankingScrollView rankingScrollView;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<IRankingScrollViewable>(rankingScrollView);
            var resultRepository = new ScoreResultRepository();
            builder.RegisterInstance<IScoreResultUseCase>(new ScoreResultUseCase(resultRepository));
            builder.RegisterEntryPoint<ScoreRankingPresenter>();
        }
    }
}
