using DG.Tweening;
using UnityEngine;

namespace Assets.Match.Scripts.UI.Animations
{
    public class GameMenuAnimation : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private RectTransform _pausePanelTransform;
        [SerializeField] private RectTransform _victoryPanelTransform;
        [SerializeField] private RectTransform _overPanelTransform;
        #endregion

        public void ForPause()
        {           
            _pausePanelTransform.DOAnchorPosY(0, 1, true);           
        }

        public void ForRestartAndContinue()
        {
            _pausePanelTransform.DOAnchorPosY(1060, 1, true);
            _overPanelTransform.DOAnchorPosX(620, 1, true);
        }

        public void ForVictory()
        {
            _victoryPanelTransform.DOAnchorPosX(0, 1, true);
        }

        public void ForGameOver()
        {
            _overPanelTransform.DOAnchorPosX(0, 1, true);
        }   

    }
}
