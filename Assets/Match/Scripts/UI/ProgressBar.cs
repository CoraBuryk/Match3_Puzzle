using Assets.Match.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private int _maxScore;
        [SerializeField] private Image _mask;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private GameObject[] _star;

        private float fillAmount;

        private void Awake()
        {
            for(int i = 0; i < _star.Length; i++)
            {
                _star[i].SetActive(false);
            }
        }

        private void OnEnable()
        {
            _scoreController.ScoreChange += GetCurrentFill;
        }

        private void OnDisable()
        {
            _scoreController.ScoreChange -= GetCurrentFill;
        }

        private void GetCurrentFill()
        {
            fillAmount = (float)_scoreController.Counter / (float)_maxScore;         
            _mask.fillAmount = fillAmount;

            CheckForStar();
        }

        private void CheckForStar()
        {
            if (fillAmount >= 0.3f)
            {
                _star[0].SetActive(true);
            }
            if (fillAmount >= 0.6f)
            {
                _star[1].SetActive(true);
            }
            if (fillAmount >= 0.9f)
            {
                _star[2].SetActive(true);
            }
        }


    }
}