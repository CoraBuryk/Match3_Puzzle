using System;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "GoalsController")]
    public class GoalController : ScriptableObject
    {
        [SerializeField] private int _valueOfGoalOne;
        [SerializeField] private int _valueOfGoalTwo;
        [SerializeField] private int _valueOfGoalThree;

        public int CounterOne {get; set;}
        public int CounterTwo {get; set;}
        public int CounterThree {get; set;}

        public event Action GoalChange;

        private void OnEnable()
        {
            CounterOne = _valueOfGoalOne;
            CounterTwo = _valueOfGoalTwo;
            CounterThree = _valueOfGoalThree;
        }

        public void ResetGoals()
        {
            ChangeGoalOne(_valueOfGoalOne);
            ChangeGoalTwo(_valueOfGoalTwo);
            ChangeGoalThree(_valueOfGoalThree);
        }

        public void ChangeGoalOne(int newGoal)
        {
            CounterOne = newGoal;
            GoalChange?.Invoke();
        }

        public void ChangeGoalTwo(int newGoal)
        {
            CounterTwo = newGoal;
            GoalChange?.Invoke();
        }

        public void ChangeGoalThree(int newGoal)
        {
            CounterThree = newGoal;
            GoalChange?.Invoke();
        }
    }
}
