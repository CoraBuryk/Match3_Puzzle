using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.UI.Animations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.Menu
{
    public class PausePanel : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject _boardPanel;
        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameMenuAnimation _gameMenuAnimation;
        [SerializeField] private ButtonAudioEffect _buttonAudioEffect;
        #endregion

        private bool _isOpen = false;

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Continue);
            _quitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(Continue);
            _quitButton.onClick.RemoveListener(Exit);
        }

        public void PauseGame()
        {
            _buttonAudioEffect.PlayClickSound();
            _gameMenuAnimation.ForPause();
            _gamePanel.SetActive(_isOpen);
            _boardPanel.SetActive(_isOpen);
        }

        private async void Continue()
        {
            _buttonAudioEffect.PlayClickSound();
            _gameMenuAnimation.ForRestartAndContinue();
            await Task.Delay(400);
            _gamePanel.SetActive(!_isOpen);
            _boardPanel.SetActive(!_isOpen);
        }

        private void Exit()
        {
            _buttonAudioEffect.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}