using UnityEngine;
using DG.Tweening;

namespace Assets.Match.Scripts.UI.Animations
{

    public class MainMenuAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _levelTransform;

        public void ForStart()
        {
            _levelTransform.DOAnchorPosX(0, 1, true);
        }

        public void KillAnimations()
        {
            _levelTransform.DOKill();
        }

    }
}