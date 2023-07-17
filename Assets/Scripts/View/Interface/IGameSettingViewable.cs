using System;
using UniRx;

namespace View.Interface
{
    public interface IGameSettingViewable
    {
       void SetNumberOfEnemies(int value);
       void SetTimeLimit(int value);
       void MoveUpViewGroup();
       void MoveDownViewGroup();
       void ShowScrollView();
       void ShowErrorAlert();
       void ShowIndicator();
       void HiddenIndicator();
       void HiddenScrollView();

       void SetOutlineSoftStar();
       void SetOutlineHardStar();
       void SetOutlineBlaveStar();
       void MoveToMainARScene();
       
       IObservable<int> OnTimeSliderValueChangedAsObservable { get; }
       IObservable<int> OnNumSliderValueChangedAsObservable { get; }
       IObservable<int> OnDropDownValueChangedAsObservable { get; }
       IObservable<Unit> OnTapSoftStarButtonAsObservable { get; }
       IObservable<Unit> OnTapHardStarButtonAsObservable { get; }
       IObservable<Unit> OnTapBlaveStarButtonAsObservable { get; }
       IObservable<string> OnPlayerNameInputFieldValueChangedAsObservable { get; }
       IObservable<Unit> OnTapGameStartButtonAsObservable { get; }
    }
}