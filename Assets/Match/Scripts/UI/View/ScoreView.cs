using Assets.Match.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Assets.Match.Scripts.UI.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private ScoreController _scoreController;

        private void OnEnable()
        {
            _scoreController.ScoreChange += Score;
        }

        private void OnDisable()
        {
            _scoreController.ScoreChange -= Score;
        }

        private void Start()
        {
            Score();
        }

        public void Score()
        {
            _score.text = $"Total score: {_scoreController.Counter}";
        }
    }
}
