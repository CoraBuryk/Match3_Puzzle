using UnityEngine;
using Assets.Match.Scripts.GeneralLevelInfo;

namespace Assets.Match.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Level Configuration")]

    public class LevelsConfigurationScriptable : ScriptableObject
    {
        [Header("current level number")]
        public int levelNumber;
        public LevelInfo level;
        public MoveInfo move;
        public GoalInfo goal;
    }
}
