using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.Models;

namespace Assets.Match.Scripts.UI.View
{

    public class GoalView : MonoBehaviour
    {

#region Serialized Variables

       // [SerializeField] private TextMeshProUGUI[] _goalCounter;
        [SerializeField] private TextMeshPro[] _goalCounter;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private List<BlockController> _blocks;

        #endregion

        public GameObject[] goals;

        private void OnEnable()
        {
            _goalController.GoalChange += Goal;
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
            System.Random rd = new System.Random();

            for (int i = _blocks.Count - 1; i >= 0; i--)
            {               
                int j = rd.Next(i + 1);
                BlockController newPos = _blocks[i];
                _blocks[i] = _blocks[j];
                _blocks[j] = newPos;
            }

            for(int i = 0;i < goals.Length; i++)
            {
                Block newGoal = Instantiate(_blocks[i], goals[i].transform.position, Quaternion.identity);
                newGoal.transform.localScale = new Vector3(0.3f, 0.3f,0);
                newGoal.transform.SetParent(goals[i].transform);
                newGoal.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                goals[i] = newGoal.gameObject;
            }
        }

        private void OnDisable()
        {
            _goalController.GoalChange -= Goal;
        }

    }
}
