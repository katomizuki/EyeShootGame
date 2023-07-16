using System;
using System.Text;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using View.Interface;

namespace View
{
    public sealed class GameSettingView : MonoBehaviour, IGameSettingViewable 
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI numText;
        [SerializeField] private Slider timeSlider;
        [SerializeField] private Slider numSlider;
        [SerializeField] private TMP_Dropdown dropDown;
        [SerializeField] private TMP_InputField plyerNameInputField;
        [SerializeField] private GameObject scrollView;
        [SerializeField] private RectTransform viewGroup;
        [SerializeField] private Button softStarButton;
        [SerializeField] private Button hardStarButton;
        [SerializeField] private Button blaveStarButton;
        [SerializeField] private Button gameStartButton;
       
        public IObservable<int> OnTimeSliderValueChangedAsObservable
            => timeSlider.OnValueChangedAsObservable().Select(value => (int)value);
        public IObservable<int> OnNumSliderValueChangedAsObservable
            => numSlider.OnValueChangedAsObservable().Select(value => (int)value);
        public IObservable<int> OnDropDownValueChangedAsObservable
            => dropDown.onValueChanged.AsObservable();
        
        public IObservable<string> OnPlayerNameInputFieldValueChangedAsObservable
            => plyerNameInputField.onValueChanged.AsObservable();
        
        public IObservable<Unit> OnTapSoftStarButtonAsObservable
            => softStarButton.OnClickAsObservable();
        public IObservable<Unit> OnTapHardStarButtonAsObservable
            => hardStarButton.OnClickAsObservable();
        public IObservable<Unit> OnTapBlaveStarButtonAsObservable
            => blaveStarButton.OnClickAsObservable();
        
        public IObservable<Unit> OnTapGameStartButtonAsObservable
            => gameStartButton.OnClickAsObservable();

        private readonly StringBuilder _stringBuilder = new();
        private Vector2 _originAnchoredPosition;
        private Material _softStarMaterial;
        private Material _hardStarMaterial;
        private Material _blaveStarMaterial;
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");

        private void Start()
        {
            _originAnchoredPosition = viewGroup.anchoredPosition;
            _softStarMaterial = softStarButton.GetComponent<RawImage>().material;
            _hardStarMaterial = hardStarButton.GetComponent<RawImage>().material;
            _blaveStarMaterial = blaveStarButton.GetComponent<RawImage>().material;
        }

        public void SetNumberOfEnemies(int value)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append("Number of Enemies: ");
            _stringBuilder.Append(value);
            numText.text = _stringBuilder.ToString();
        }

        public void SetTimeLimit(int value)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append("Game Time Limit: ");
            _stringBuilder.Append(value);
            _stringBuilder.Append("s");
            timeText.text = _stringBuilder.ToString();
        }
        
        public void MoveUpViewGroup()
        {
           viewGroup.anchoredPosition = new Vector2(0, 200); 
        }
        
        public void MoveDownViewGroup()
        {
           viewGroup.anchoredPosition = _originAnchoredPosition; 
        }
        
        public void ShowScrollView()
        {
            scrollView.SetActive(true);
        }
        
        public void HiddenScrollView()
        {
            scrollView.SetActive(false);
        }
        
        public void SetOutlineSoftStar()
        {
            _softStarMaterial.SetFloat(OutlineWidth,7f);
            _hardStarMaterial.SetFloat(OutlineWidth,0f);
            _blaveStarMaterial.SetFloat(OutlineWidth,0f);
        }
        
        public void SetOutlineHardStar()
        {
            _softStarMaterial.SetFloat(OutlineWidth,0f);
            _hardStarMaterial.SetFloat(OutlineWidth,7f);
            _blaveStarMaterial.SetFloat(OutlineWidth,0f);
        }
        
        public void SetOutlineBlaveStar()
        {
            _softStarMaterial.SetFloat(OutlineWidth,0f);
            _hardStarMaterial.SetFloat(OutlineWidth,0f);
            _blaveStarMaterial.SetFloat(OutlineWidth,7f);
        }
        
        public void MoveToMainARScene()
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
    }
}
