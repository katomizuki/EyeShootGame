using System;
using UniRx;

namespace View.Interface
{
    public interface IScoreViewable
    {
        void SetScoreText(string text);
        void SetCountDownTimeText(float value);
        void MoveToEndScreen();
        void ShowResultScene(float value);
        void ShowTimeUpAlert();
        void ShowErrorAlert();
        void OnGameClearAction();
        void ShowIndicator();
        void HideIndicator();
        void StopTimer();
        IObservable<Unit> OnTapCloseButton { get; }
        IObservable<Unit> OnTimeUp { get; }
        IObservable<float> OnGameClear { get; }
    }
}