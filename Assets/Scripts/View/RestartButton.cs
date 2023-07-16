using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

namespace View
{
    [RequireComponent(typeof(Button))]
    internal sealed class RestartButton : MonoBehaviour
    {
        private void Start()
        {
            var restartButton = GetComponent<Button>();
            restartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    MoveToSettingScene();
                })
                .AddTo(this);
        }

        private void MoveToSettingScene()
        {
            SceneManager.LoadSceneAsync("GameSettingScene");
        }
    }
}
