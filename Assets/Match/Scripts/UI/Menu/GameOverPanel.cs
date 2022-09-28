using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.UI.Animations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.Menu
{
    public class GameOverPanel : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private Button _restartLevelButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private GameObject _board;
        [SerializeField] private StarController _starController;
        [SerializeField] private GameMenuAnimation _gameMenuAnimation;
        [SerializeField] private GameController _gameController;
        [SerializeField] private ButtonAudioEffect _buttonAudioEffect;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;
        [SerializeField] private BoardManager _boardManager;
        #endregion

        private bool _isOpen = false;
        public bool IsOverState { get; set; } = false;

        private void OnEnable()
        {
            _restartLevelButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(ToStartMenu);
        }

        private void OnDisable()
        {
            _restartLevelButton.onClick.RemoveListener(Restart);
            _exitButton.onClick.RemoveListener(ToStartMenu);
        }

        private void Restart()
        {
            IsOverState = false;
            _boardManager.UpdateBoard();
            _buttonAudioEffect.PlayClickSound();
            _board.SetActive(!_isOpen);
            _gameMenuAnimation.ForRestartAndContinue();
            _gameController.Restart();
        }

        private void ToStartMenu()
        {
            _buttonAudioEffect.PlayClickSound();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public async void GameOverState()
        {
            IsOverState = true;
            _starController.ResetStar();
            await Task.Delay(800);
            _audioEffectsGame.PlayLoseSound();           
            _gameMenuAnimation.ForGameOver();
            _board.SetActive(_isOpen);
        }
    }
}
