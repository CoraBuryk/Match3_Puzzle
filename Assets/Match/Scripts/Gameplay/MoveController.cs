using System;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    public class MoveController : MonoBehaviour
    {
        public int TotalMove { get; set; } = 20;
        public event Action MovesChange;

        public void NumberOfMoves(int moves)
        {
            TotalMove = moves;
            MovesChange?.Invoke();
        }
    }
}
