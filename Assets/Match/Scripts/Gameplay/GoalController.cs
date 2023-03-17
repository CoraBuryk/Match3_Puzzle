using System;
using UnityEngine;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class GoalController : MonoBehaviour
    {
#if (UNITY_EDITOR)
        [RequiredField]
#endif
        [SerializeField] private LevelsConfigurationScriptable _levelConfig;
        public int CounterOne {get; private set;}

        public int CounterTwo {get; private set;}

        public int CounterThree {get; private set;}

        public event Action GoalChange;

        private void Awake()
        {
            CounterOne = _levelConfig.goal.valueOfGoalOne;
            CounterTwo = _levelConfig.goal.valueOfGoalTwo;
            CounterThree = _levelConfig.goal.valueOfGoalThree;
        }

        public void ResetGoals()
        {
            ChangeGoals(_levelConfig.goal.valueOfGoalOne, 1);
            ChangeGoals(_levelConfig.goal.valueOfGoalTwo, 2);
            ChangeGoals(_levelConfig.goal.valueOfGoalThree, 3);
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
