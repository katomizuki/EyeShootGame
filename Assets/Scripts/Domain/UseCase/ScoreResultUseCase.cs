using System;
using Domain.DTO;
using Domain.IRepository;
using Domain.UseCase.Interface;
using UniRx;

namespace Domain.UseCase
{
    public sealed class ScoreResultUseCase: IScoreResultUseCase
    {
        private readonly IResultRepository _resultRepository;
        
        public ScoreResultUseCase(IResultRepository resultRepository)
        {
            this._resultRepository = resultRepository;
        }
        
        public IObservable<ScoreDto[]> FetchScoreResult()
        {
            return Observable.FromCoroutine<ScoreDto[]>(
                observer =>
                    _resultRepository.GetResult(observer));
        }
    }
}