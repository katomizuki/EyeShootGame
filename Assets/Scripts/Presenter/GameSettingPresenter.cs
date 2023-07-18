using System;
using Domain.Model;
using Domain.UseCase.Interface;
using UniRx;
using VContainer.Unity;
using View.Interface;

namespace Presenter
{
    public sealed class GameSettingPresenter: IPostInitializable, IDisposable
    {
        private readonly IGameSettingUseCase _useCase;
        private readonly CompositeDisposable _disposable = new();
        private readonly IGameSettingViewable _gameSettingView; 
        
        GameSettingPresenter(
            IGameSettingUseCase useCase,
            IGameSettingViewable gameSettingView)
        {
            this._useCase = useCase;
            this._gameSettingView = gameSettingView;
        }
        
        public void PostInitialize()
        {
            SetupPrefab();
            SubscribeDropDown();
            SubscribeNumSlider();
            SubscribeTimeSlider();
            SubscribePlayerNameInputField();
            SubscribeBulletSelectionButton();
            SubscribeGameStartButton();
        }
        
        private async void SetupPrefab()
        {
            _gameSettingView.ShowIndicator();
            var result = await _useCase.SetupPrefab();
            _gameSettingView.HiddenIndicator();
            if (!result)
                _gameSettingView.ShowErrorAlert();
        }

        private void SubscribeDropDown()
        {
            _gameSettingView.OnDropDownValueChangedAsObservable
                .Subscribe(index =>
                {
                    var mode = (GameAttackMode)index;
                    _useCase.ChangeGameAttackMode(mode);
                    switch (mode)
                    {
                        case GameAttackMode.Bullet:
                            _gameSettingView.MoveDownViewGroup();
                            _gameSettingView.ShowScrollView();
                            break;
                        case GameAttackMode.Lazer:
                            _gameSettingView.MoveUpViewGroup();
                            _gameSettingView.HiddenScrollView();
                            break;
                    }
                })
                .AddTo(_disposable);  
        }
        
        private void SubscribeNumSlider()
        {
            _gameSettingView.OnNumSliderValueChangedAsObservable
                .Subscribe(numberOfEnemies =>
                {
                    _useCase.ChangeNumberOfEnemies(numberOfEnemies);
                    _gameSettingView.SetNumberOfEnemies(numberOfEnemies);
                })
                .AddTo(_disposable); 
        }
        
        private void SubscribeTimeSlider()
        {
            _gameSettingView.OnTimeSliderValueChangedAsObservable
                .Subscribe(timeLimit =>
                {
                    _useCase.ChangeGameTimeLimit(timeLimit);
                    _gameSettingView.SetTimeLimit(timeLimit);
                })
                .AddTo(_disposable); 
        }
        
        private void SubscribePlayerNameInputField()
        {
            _gameSettingView.OnPlayerNameInputFieldValueChangedAsObservable
                .Subscribe(playerName =>
                {
                    _useCase.ChangePlayerName(playerName);
                })
                .AddTo(_disposable); 
        }

        private void SubscribeBulletSelectionButton()
        {
            _gameSettingView
                .OnTapBlaveStarButtonAsObservable
                .Subscribe(_ =>
                {
                    var bullet = StarBullet.Beveled;
                    _useCase.ChangeStarBullet(bullet);
                    _gameSettingView.SetOutlineBlaveStar();
                })
                .AddTo(_disposable);

            _gameSettingView
                .OnTapHardStarButtonAsObservable
                .Subscribe(_ =>
                {
                    var bullet = StarBullet.Hard;
                    _useCase.ChangeStarBullet(bullet);
                    _gameSettingView.SetOutlineHardStar();
                })
                .AddTo(_disposable);

            _gameSettingView
                .OnTapSoftStarButtonAsObservable
                .Subscribe(_ =>
                {
                    var bullet = StarBullet.Soft;
                    _useCase.ChangeStarBullet(bullet);
                    _gameSettingView.SetOutlineSoftStar();
                })
                .AddTo(_disposable);
        }
        
        private void SubscribeGameStartButton()
        {
            _gameSettingView.OnTapGameStartButtonAsObservable
                .Subscribe(_ =>
                {
                    _useCase.ReleaseUnNeededGameObjects();
                    _gameSettingView.MoveToMainARScene();
                })
                .AddTo(_disposable); 
        }
        
        public void Dispose()
        {
           _disposable.Dispose(); 
        }
    }
}