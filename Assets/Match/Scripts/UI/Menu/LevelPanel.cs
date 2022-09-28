using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.UI.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.Menu
{
    public class LevelPanel : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private Button _firstLevelButton;
        [SerializeField] private Button _secondLevelButton;
        [SerializeField] private Button _thirdLevelButton;
        [SerializeField] private Button _fourthLevelButton;
        [SerializeField] private MainMenuAnimation _mainMenuAnimation;
        [SerializeField] private ButtonAudioEffect _audioEffectsStartScene;
        #endregion

        private void OnEnable()
        {
            _firstLevelButton.onClick.AddListener(StartLevelOne);
            _secondLevelButton.onClick.AddListener(StartLevelTwo);
            _thirdLevelButton.onClick.AddListener(StartLevelThree);
            _fourthLevelButton.onClick.AddListener(StartLevelFour);
        }

        private void OnDisable()
        {
            _firstLevelButton.onClick.RemoveListener(StartLevelOne);
            _secondLevelButton.onClick.RemoveListener(StartLevelTwo);
            _thirdLevelButton.onClick.RemoveListener(StartLevelThree);
            _fourthLevelButton.onClick.RemoveListener(StartLevelFour);
        }

        private void StartLevelOne()
        {
            _audioEffectsStartScene.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            _mainMenuAnimation.KillAnimations();
        }

        private void StartLevelTwo()
        {
            _audioEffectsStartScene.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            _mainMenuAnimation.KillAnimations();
        }

        private void StartLevelThree()
        {
            _audioEffectsStartScene.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
            _mainMenuAnimation.KillAnimations();
        }

        private void StartLevelFour()
        {
            _audioEffectsStartScene.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
            _mainMenuAnimation.KillAnimations();
        }
    }
}
