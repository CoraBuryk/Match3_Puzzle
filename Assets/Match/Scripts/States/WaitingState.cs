using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.InputSystemController;

namespace Assets.Match.Scripts.States
{
    public class WaitingState : State
    {
        protected BoardManager _board;
        protected InputManager _inputManager;

        protected virtual void Awake()
        {
            _board = GetComponent<BoardManager>();
            _inputManager = InputManager.Instance;
        }

        public override void Enter()
        {
            base.Enter();
            _inputManager.OnPress.AddListener(_board.SelectTiles);
        }

        public override void Exit()
        {
            base.Exit();
            _inputManager.OnPress.RemoveListener(_board.SelectTiles);
        }
    }
}
