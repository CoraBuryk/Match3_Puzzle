using System;
using UnityEngine;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class MoveController : MonoBehaviour
    {
        [SerializeField] private MoveScriptableObjects _move;

        public int TotalMove { get; private set; }

        public event Action MovesChange;

        private void Awake()
        {
            TotalMove = _move.maxMove;
        }

        public void ResetMoves()
        {
            NumberOfMoves(_move.maxMove);
        }

        public void NumberOfMoves(int moves)
        {
            TotalMove = moves;
            MovesChange?.Invoke();
        }

    }
}
