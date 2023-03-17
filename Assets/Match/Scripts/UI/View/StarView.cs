using UnityEngine;
using UnityEngine.UI;
using Assets.Match.Scripts.Gameplay;

namespace Assets.Match.Scripts.UI.View
{

    public class StarView : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _fullStar;
        [SerializeField] private Sprite _emptyStar;

        [SerializeField] private StarController _starController;

#endregion

        private void OnEnable()
        {
            _starController.StarChange += NumberOfStars;
        }
       
        private void Start()
        {
            NumberOfStars();
        }

        public void NumberOfStars()
        {
            for(int i = 0; i < _stars.Length; i++)
            {
                if(i < _starController.NumOfStar)
                {
                    _stars[i].sprite = _fullStar;
                }
                else
                {
                    _stars[i].sprite = _emptyStar;
                }
            }
        }

        private void OnDisable()
        {
            _starController.StarChange -= NumberOfStars;
        }

    }
}
