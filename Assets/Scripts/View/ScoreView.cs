using System;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Interface;

namespace View
{
    public sealed class ScoreView : MonoBehaviour, IScoreViewable
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeLimitText;

        private readonly Subject<Unit> _onTimeUp = new();
        private readonly Subject<Unit> _onTapCloseButton = new();
        private readonly Subject<float> _onGameClear = new();
        private readonly StringBuilder _stringBuilder = new();
        
        private float _countDownTime;
        private bool _isTimeUp;

        [DllImport("__Internal")]
        private static extern void present_alert(string title, string message);
        
        [DllImport("__Internal")]
        private static extern void present_indicator();
        [DllImport("__Internal")]
        private static extern void hide_indicator();
        public IObservable<Unit> OnTimeUp => _onTimeUp;
        public IObservable<Unit> OnTapCloseButton => _onTapCloseButton;
        public IObservable<float> OnGameClear => _onGameClear;

        private void Awake()
        {
            _isTimeUp = false;
        }

        public void SetScoreText(string text)
        {
            scoreText.text = text;
        }

        public void SetCountDownTimeText(float value)
        {
            _countDownTime = value;
        }
        
        // Nativeからコールバックを受け取る。
        public void OnTapCloseButtonAction(string args)
        {
            _onTapCloseButton.OnNext(Unit.Default);
        }
        
        public void OnGameClearAction()
        {
            _onGameClear.OnNext(_countDownTime);
        }

        private void Update()
        {
            if (_isTimeUp) return;
            _countDownTime -= Time.deltaTime;
            ChangeTimeLimitText();
          
            if (_countDownTime <= 0)
                _onTimeUp.OnNext(Unit.Default);
        }

        private void ChangeTimeLimitText()
        {
            _stringBuilder.Clear();
            _stringBuilder.Append("Time Limit: ");
            _stringBuilder.Append(((int)_countDownTime));
            _stringBuilder.Append("s");
            timeLimitText.text = _stringBuilder.ToString(); 
        }

        public void StopTimer()
        {
            _isTimeUp = true;
        }
        
        public void ShowResultScene(float value)
        {
           present_alert("Game Clear!!", $"Your Score is {value} (Number destroyed per second)"); 
        }

        public void ShowTimeUpAlert()
        {
            present_alert("Time Up!!", "Game Over");
        }
        
        public void ShowErrorAlert()
        {
            present_alert("Error!!", "Please check your network connection");
        }
        
        public void MoveToEndScreen()
        {
            SceneManager.LoadSceneAsync("ScoreResultScene");
        }
        
        public void ShowIndicator()
        {
            present_indicator();
        }

        public void HideIndicator()
        {
            hide_indicator();
        }
    }

}