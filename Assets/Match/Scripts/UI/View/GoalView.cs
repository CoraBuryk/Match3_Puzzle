using Assets.Match.Scripts.Gameplay;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.View
{
    public class GoalView : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private TextMeshProUGUI[] _goalCounter;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private List<Sprite> _blockSprites;      
        #endregion

        public Image[] goalImage;

        private void OnEnable()
        {
            _goalController.GoalChange += Goal;
        }

        private void OnDisable()
        {
            _goalController.GoalChange -= Goal;
        }

        private void Start()
        {
            SetRandomGoal();
            Goal();
        }

        private void Goal()
        {
            _goalCounter[0].text = _goalController.CounterOne.ToString();
            _goalCounter[1].text = _goalController.CounterTwo.ToString();
            _goalCounter[2].text = _goalController.CounterThree.ToString();
        }

        private void SetRandomGoal()
        {
            for(int i= 0; i < goalImage.Length; i++)
            {
                int randomBlock = Random.Range(0, _blockSprites.Count);
                Sprite newGoal = _blockSprites[randomBlock];

                if (newGoal != goalImage[0].sprite && newGoal != goalImage[1].sprite && newGoal != goalImage[2].sprite)
                    goalImage[i].sprite = newGoal;
                else
                {
                    int newRandomBlock = Random.Range(0, _blockSprites.Count);
                    goalImage[i].sprite = _blockSprites[newRandomBlock];
                }
            }
        }
    }
}
