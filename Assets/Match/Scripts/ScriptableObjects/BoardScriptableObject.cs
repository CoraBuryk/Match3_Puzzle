using UnityEngine;
using Assets.Match.Scripts.Models;

namespace Assets.Match.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Board")]

    public class BoardScriptableObject: ScriptableObject
    {
        public int XSize { get; set; }

        public int YSize { get; set; }

        public Tiles[,] Tiles { get; set; }

        public Block[,] Blocks { get; set; }

        public Obstacles[,] Obstacles { get; set; }

    }
}
