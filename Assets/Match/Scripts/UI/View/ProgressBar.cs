using UnityEngine;
using UnityEngine.UI;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.UI.View
{

    public class ProgressBar : MonoBehaviour
    {

        #region Serialized Variables

#if (UNITY_EDITOR)
        [RequiredField]
#endif
        [SerializeField] private LevelsConfigurationScriptable _levelConfig;
        [SerializeField] private Image _mask;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private StarController _starController;
        [SerializeField] private GameObject _starParticle;
        [SerializeField] private RectTransform[] _starsTransfom;

#endregion

        public float fillAmount;

        private void Awake()
        {
            _mask.fillAmount = 0;
        }

        private void OnEnable()
        {
            _scoreController.ScoreChange += GetCurrentFill;
            _starController.StarChange += CheckForStar;
        }

        private void GetCurrentFill()
        {
            fillAmount = (float)_scoreController.Counter / (float)_levelConfig.level.maxScore;         
            _mask.fillAmount = fillAmount;

            CheckForStar();
        }

        private void CheckForStar()
        {
            GameObject particle;
            
            if (fillAmount >= 0.3f && fillAmount < 0.6f && _starController.NumOfStar == 0 )
            { 
                _starController.StarIncrease(_starController.NumOfStar = 1);
                particle = Instantiate(_starParticle, _starsTransfom[0].position, Quaternion.identity);
                Destroy(particle, 3f);
            }
            if (fillAmount >= 0.6f && fillAmount < 0.9f && _starController.NumOfStar == 1)
            {
                _starController.StarIncrease(_starController.NumOfStar = 2);
                particle = Instantiate(_starParticle, _starsTransfom[1].position, Quaternion.identity);
                Destroy(particle, 3f);
            }
            if (fillAmount >= 0.95f && _starController.NumOfStar == 2)
            {
                _starController.StarIncrease(_starController.NumOfStar = 3);
                particle = Instantiate(_starParticle, _starsTransfom[2].position, Quaternion.identity);
                Destroy(particle, 3f);
            }
        }

        private void OnDisable()
        {
            _scoreController.ScoreChange -= GetCurrentFill;
            _starController.StarChange -= CheckForStar;
        }

    }
}