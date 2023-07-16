using System;
using System.Collections;
using Domain.DTO;
using UniRx;

namespace Domain.IRepository
{
    public interface IResultRepository
    {
        IEnumerator PostResult(IObserver<Unit> observer, float score, string name);
        IEnumerator GetResult(IObserver<ScoreDto[]> observer);
    }
}