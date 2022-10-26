using System;
using UnityEngine;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class GoalController : MonoBehaviour
    {
        [SerializeField] private GoalScriptableObject _goal;

        public int CounterOne {get; private set;}

        public int CounterTwo {get; private set;}

        public int CounterThree {get; private set;}

        public event Action GoalChange;

        private void Awake()
        {
            CounterOne = _goal.valueOfGoalOne;
            CounterTwo = _goal.valueOfGoalTwo;
            CounterThree = _goal.valueOfGoalThree;
        }

        public void ResetGoals()
        {
            ChangeGoalOne(_goal.valueOfGoalOne);
            ChangeGoalTwo(_goal.valueOfGoalTwo);
            ChangeGoalThree(_goal.valueOfGoalThree);
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
