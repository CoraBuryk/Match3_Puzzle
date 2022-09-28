using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.UI.Animations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.Menu
{
    public class VictoryPanel : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private ButtonAudioEffect _buttonAudioEffect;
        [SerializeField] private AudioEffectsGame _audioEffectsGame;
        [SerializeField] private StarController _starController;
        [SerializeField] private GameMenuAnimation _gameMenuAnimation;
        [SerializeField] private ObjectsAnimation _objectsAnimation;

        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private GameObject _gamePanel;
        [SerializeField] private GameObject _board;

        [SerializeField] private GameObject[] _starPosition;
        [SerializeField] private GameObject _starPref;
        #endregion

        private bool _isOpen = false;

        public bool IsVictoryState { get; set; } = false;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(ToNextLevel);
            _exitButton.onClick.AddListener(ToStartMenu);
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(ToNextLevel);
            _exitButton.onClick.RemoveListener(ToStartMenu);
        }

        private void ToNextLevel()
        {
            _buttonAudioEffect.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void ToStartMenu()
        {
            _buttonAudioEffect.PlayClickSound();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public async void VictoryState()
        {
            IsVictoryState = true;
            await Task.Delay(1500);
            _audioEffectsGame.PlayVictorySound();
            _gameMenuAnimation.ForVictory();
            _board.SetActive(_isOpen);
            CheckNumberOfStars();
        }

        private void CheckNumberOfStars()
        {
            GameObject[] totalStars = new GameObject[3];

            if (_starController.NumOfStar == 1)
            {
                totalStars[0] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[0].transform);
                _objectsAnimation.Stars(totalStars[0], _starPosition[0].transform.position.x, _starPosition[0].transform.position.y, 1000);
            }
            if (_starController.NumOfStar == 2)
            {
                totalStars[0] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[0].transform);
                _objectsAnimation.Stars(totalStars[0], _starPosition[0].transform.position.x, _starPosition[0].transform.position.y, 1000);

                totalStars[1] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[1].transform);
                _objectsAnimation.Stars(totalStars[1], _starPosition[1].transform.position.x, _starPosition[1].transform.position.y, 2000);
            }
            if (_starController.NumOfStar == 3)
            {
                totalStars[0] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[0].transform);
                _objectsAnimation.Stars(totalStars[0], _starPosition[0].transform.position.x, _starPosition[0].transform.position.y, 1000);

                totalStars[1] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[1].transform);
                _objectsAnimation.Stars(totalStars[1], _starPosition[1].transform.position.x, _starPosition[0].transform.position.y, 2000);

                totalStars[2] = Instantiate(_starPref, transform.position, Quaternion.identity, _starPosition[2].transform);
                _objectsAnimation.Stars(totalStars[2], _starPosition[2].transform.position.x, _starPosition[0].transform.position.y, 3000);
            }
        }
        
    }
}
