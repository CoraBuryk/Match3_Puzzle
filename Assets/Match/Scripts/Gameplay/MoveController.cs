using System;
using UnityEngine;


namespace Assets.Match.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "MoveController")]
    public class MoveController : ScriptableObject
    {
        [SerializeField] private int _maxMove;

        public int TotalMove { get; private set; }

        public event Action MovesChange;

        private void OnEnable()
        {
            TotalMove = _maxMove;
        }

        public void ResetMoves()
        {
            NumberOfMoves(_maxMove);
        }

        public void NumberOfMoves(int moves)
        {
            TotalMove = moves;
            MovesChange?.Invoke();
        }
    }
}
