using System;
using Domain.DTO;
using Domain.IRepository;
using Domain.UseCase.Interface;
using UniRx;

namespace Domain.UseCase
{
    public sealed class ScoreResultUseCase: IScoreResultUseCase
    {
        private readonly IScoreResultRepository _scoreResultRepository;
        
        public ScoreResultUseCase(IScoreResultRepository scoreResultRepository)
        {
            this._scoreResultRepository = scoreResultRepository;
        }
        
        public IObservable<ScoreDto[]> FetchScoreResult()
        {
            return Observable.FromCoroutine<ScoreDto[]>(
                observer =>
                    _scoreResultRepository.GetResult(observer));
        }
    }
}