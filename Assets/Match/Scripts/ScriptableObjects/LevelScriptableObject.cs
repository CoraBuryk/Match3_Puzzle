using Assets.Match.Scripts.Enum;
using System;
using UnityEngine;

namespace Assets.Match.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level")]
    public class LevelScriptableObject : ScriptableObject
    {
        public bool isConfigured;
        [Header("x size of board. Min size = 3, max size = 7")]
        public int xSize;
        [Header("y size of board. Min size = 3, max size = 11")]
        public int ySize;
        public int maxScore;
        public bool isObstacleLevel;
        public bool isBonusLevel;
        public ObstaclesOnLevel obstaclesOnLevel;
        public int totalStar;

        [Serializable]
        public struct ObstaclesOnLevel
        {
            public ObstacleType type;

            public int NumberOfObstacles;

            [Header("x and y position of obstacles on level. if == 0 - random position")]

            public Vector3Int[] position;
        }
    }
}
