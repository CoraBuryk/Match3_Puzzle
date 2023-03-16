using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Match.Scripts.Ads;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.UI.Animations;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.Enum;

namespace Assets.Match.Scripts.UI.Menu
{

    public class LevelPanel : MonoBehaviour
    {

        #region Serialized Variables

#if (UNITY_EDITOR)
        [RequiredField(FieldColor.Yellow)]
#endif
        [SerializeField] private Button[] _levelsButtons;

        [SerializeField] private MainMenuAnimation _mainMenuAnimation;
        [SerializeField] private ButtonAudioEffect _audioEffectsStartScene;
        [SerializeField] private InterstitialAds _interstitialAds;

#endregion

        private void OnEnable()
        {
            for(int i = 0; i < _levelsButtons.Length; i++)
            {
                int index = i + 1;
                _levelsButtons[i].onClick.AddListener(_interstitialAds.ShowAd);
                _levelsButtons[i].onClick.AddListener(() => StartLevel(index));
            }
        }

        private void StartLevel(int index)
        {
            _audioEffectsStartScene.PlayClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
            _mainMenuAnimation.KillAnimations();
        }

        private void OnDisable()
        {
            foreach (Button button in _levelsButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

    }
}
