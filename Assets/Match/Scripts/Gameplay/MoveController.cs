using System;
using UnityEngine;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class MoveController : MonoBehaviour
    {
#if (UNITY_EDITOR)
        [RequiredField]
#endif
        [SerializeField] private LevelsConfigurationScriptable _levelConfig;

        public int TotalMove { get; private set; }

        public event Action MovesChange;

        private void Awake()
        {
            TotalMove = _levelConfig.move.maxMove;
        }

        public void ResetMoves()
        {
            NumberOfMoves(_levelConfig.move.maxMove);
        }

        public void NumberOfMoves(int moves)
        {
            TotalMove = moves;
            MovesChange?.Invoke();
        }

    }
}
