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
            ChangeGoals(_goal.valueOfGoalOne, 1);
            ChangeGoals(_goal.valueOfGoalTwo, 2);
            ChangeGoals(_goal.valueOfGoalThree, 3);
        }

        public void ChangeGoals(int newGoal, int counter)
        {
            if(counter == 1)
            {
                CounterOne = newGoal;          
            }
            if (counter == 2)
            {
                CounterTwo = newGoal;
            }
            if (counter == 3)
            {
                CounterThree = newGoal;
            }

            GoalChange?.Invoke();
        }

    }
}
