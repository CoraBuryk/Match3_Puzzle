using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.UI.Animations;

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
       
        public void PauseGame()
        {
            _buttonAudioEffect.PlayClickSound();
            _gameMenuAnimation.ForPause();
            _gamePanel.SetActive(_isOpen);
            _boardPanel.SetActive(_isOpen);
        }

        private async void Continue()
        {
            try
            {
                _buttonAudioEffect.PlayClickSound();
                _gameMenuAnimation.ForRestartAndContinue();
                await Task.Delay(400);
                _gamePanel.SetActive(!_isOpen);
                _boardPanel.SetActive(!_isOpen);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                throw;
            }           
        }

        private void Exit()
        {
            _buttonAudioEffect.PlayClickSound();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(Continue);
            _quitButton.onClick.RemoveListener(Exit);
        }
        
    }
}