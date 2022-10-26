using UnityEngine;

namespace Assets.Match.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Goals")]

    public class GoalScriptableObject : ScriptableObject
    {
        public int valueOfGoalOne;
        public int valueOfGoalTwo;
        public int valueOfGoalThree;
    }
}
