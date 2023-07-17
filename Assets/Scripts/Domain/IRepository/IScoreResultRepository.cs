using System;
using System.Collections;
using Domain.DTO;
using UniRx;

namespace Domain.IRepository
{
    public interface IScoreResultRepository
    {
        IEnumerator PostResult(IObserver<Unit> observer, float score, string name);
        IEnumerator GetResult(IObserver<ScoreDto[]> observer);
    }
}