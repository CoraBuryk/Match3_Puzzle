using UnityEngine;
using DG.Tweening;

namespace Assets.Match.Scripts.UI.Animations
{

    public class GameMenuAnimation : MonoBehaviour
    {
#region Serialized Variables

        [SerializeField] private RectTransform _pausePanelTransform;
        [SerializeField] private RectTransform _victoryPanelTransform;
        [SerializeField] private RectTransform _overPanelTransform;
        [SerializeField] private RectTransform _boardTransform;
        [SerializeField] private RectTransform _goalPanelTransform;

#endregion

        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
        }

        public void ForPause()
        {           
            _pausePanelTransform.DOAnchorPosY(0, 1, true);           
        }

        public void ForRestartAndContinue()
        {
            _sequence.Append(_overPanelTransform.DOAnchorPosX(620, 1, true));
            _sequence.Append(_pausePanelTransform.DOAnchorPosY(1060, 1, true));          
            _sequence.Append(_boardTransform.DOAnchorPosY(0, 2, false));
            _sequence.Append(_goalPanelTransform.DOAnchorPosY(-135f, 2.5f, true));
        }

        public void ForVictory()
        {
            _sequence.Insert(2f,_victoryPanelTransform.DOAnchorPosX(0, 1, true));
            _sequence.Append(_goalPanelTransform.DOAnchorPosY(1060, 1, true));
            _sequence.Append(_boardTransform.DOAnchorPosY(50, 1, false));
        }

        public void ForGameOver()
        {
            _sequence.Insert(2f,_overPanelTransform.DOAnchorPosX(0, 1, true));
            _sequence.Append(_goalPanelTransform.DOAnchorPosY(1060, 1, true));
            _sequence.Append(_boardTransform.DOAnchorPosY(50, 1, false));
        }   

    }
}
