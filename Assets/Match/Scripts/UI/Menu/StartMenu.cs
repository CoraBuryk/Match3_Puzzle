using UnityEngine;
using UnityEngine.UI;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.UI.Animations;

namespace Assets.Match.Scripts.UI.Menu
{

    public class StartMenu : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private MainMenuAnimation _mainMenuAnimation;
        [SerializeField] private ButtonAudioEffect _audioEffectsStartScene;

#endregion

        private void OnEnable()
        {           
            _startButton.onClick.AddListener(StartGame);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void StartGame()
        {
            _audioEffectsStartScene.PlayClickSound();
            _mainMenuAnimation.ForStart();
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _quitButton.onClick.RemoveListener(QuitGame);
        }

        private void QuitGame()
        {
            _audioEffectsStartScene.PlayClickSound();
            Application.Quit();
        }

    }
}