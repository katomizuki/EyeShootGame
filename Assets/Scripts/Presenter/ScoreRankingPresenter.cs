using System;
using System.Linq;
using Domain.UseCase.Interface;
using UniRx;
using VContainer;
using VContainer.Unity;
using View.Interface;
using View.ViewData;

namespace Presenter
{
    public sealed class ScoreRankingPresenter: IPostInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        // View
        private readonly IRankingScrollViewable _rankingScrollViewable;
        // UseCase
        private readonly IScoreResultUseCase _useCase;

        [Inject]
        ScoreRankingPresenter(
            IRankingScrollViewable rankingScrollViewable,
            IScoreResultUseCase useCase)
        {
            this._rankingScrollViewable = rankingScrollViewable;
            this._useCase = useCase;
        }
        
        public void PostInitialize()
        {
           FetchScores(); 
        }

        private void FetchScores()
        {
            _rankingScrollViewable.ShowIndicator();
            _useCase.FetchScoreResult()
                .Subscribe(result =>
                {
                    _rankingScrollViewable.HideIndicator();
                    var scoreViewData = result
                        .OrderByDescending(score => score.Value)
                        .Select((score, index) => new RankTextViewData(score.Value, score.PlayerName, index))
                        .ToList();
                    _rankingScrollViewable.ShowScoreResults(scoreViewData);  
                },_ => _rankingScrollViewable.ShowErrorAlert())
                .AddTo(_disposable);
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}