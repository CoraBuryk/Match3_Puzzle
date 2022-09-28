using DG.Tweening;
using UnityEngine;

namespace Assets.Match.Scripts.UI.Animations
{
    public class MainMenuAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _levelTransform;
        [SerializeField] private RectTransform _startTransform;

        public void ForStart()
        {
            _levelTransform.DOAnchorPosX(0, 1, true);
            _startTransform.DOAnchorPosX(340, 1, true);
        }

        public void KillAnimations()
        {
            _levelTransform.DOKill();
            _startTransform.DOKill();
        }
    }
}