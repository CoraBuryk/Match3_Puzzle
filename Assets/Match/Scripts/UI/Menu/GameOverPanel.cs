using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Match.Scripts.Ads;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.UI.Animations;

namespace Assets.Match.Scripts.UI.Menu
{

    public class GameOverPanel : MonoBehaviour
    {
#region Serialized Variables

        [SerializeField] private Button _restartLevelButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private StarController _starController;
        [SerializeField] private GameMenuAnimation _gameMenuAnimation;
        [SerializeField] private GameController _gameController;
        [SerializeField] private ButtonAudioEffect _buttonAudioEffect;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;
        [SerializeField] private BoardManager _boardManager;
        [SerializeField] private RewardedAds _rewardedAds;

        [SerializeField] private GameObject _gamePanel;

#endregion

        public bool IsOverState { get; set; } = false;
        private bool _isOpen = false;

        private void OnEnable()
        {
            _restartLevelButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(ToStartMenu);
        }       

        private void Restart()
        {
            IsOverState = false;

            _gamePanel.SetActive(!_isOpen);
            _starController.ResetStar();          
            _boardManager.UpdateBoard();
            _buttonAudioEffect.PlayClickSound();
            _gameMenuAnimation.ForRestartAndContinue();
            _gameController.Restart();
            _rewardedAds.LoadAd();
        }

        private async void ToStartMenu()
        {
            try
            {
                _buttonAudioEffect.PlayClickSound();
                _starController.ResetStar();
                await Task.Delay(200);
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        public async void GameOverState()
        {
            try
            {
                IsOverState = true;
                await Task.Delay(600);
                _gamePanel.SetActive(_isOpen);
                _audioEffectsGame.PlayLoseSound();
                _gameMenuAnimation.ForGameOver();
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        private void OnDisable()
        {
            _restartLevelButton.onClick.RemoveListener(Restart);
            _exitButton.onClick.RemoveListener(ToStartMenu);
        }
    }
}
