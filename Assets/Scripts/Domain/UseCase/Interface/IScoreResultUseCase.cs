using System;
using Domain.DTO;

namespace Domain.UseCase.Interface
{
    public interface IScoreResultUseCase
    {
       IObservable<ScoreDto[]> FetchScoreResult();
    }
}