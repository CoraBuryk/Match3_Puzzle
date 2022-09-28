using Assets.Match.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI.Menu
{
    public class SceenBehavior : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PausePanel _pausePanel;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private GameOverPanel _gameOverPanel;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private VictoryPanel _victoryPanel;
        [SerializeField] private GameController _gameController;
        #endregion

        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(_pausePanel.PauseGame);
            _moveController.MovesChange += GameOver;
            _goalController.GoalChange += GoalReached;
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(_pausePanel.PauseGame);
            _moveController.MovesChange -= GameOver;
            _goalController.GoalChange -= GoalReached;
        }

        public void GameOver()
        {
            if (_moveController.TotalMove < 0)
            {
                _gameOverPanel.GameOverState();
            }
        }

        public void GoalReached()
        {
            if (_goalController.CounterOne <= 0 && _goalController.CounterTwo <= 0 && _goalController.CounterThree <= 0
                && _moveController.TotalMove >= 0)
            {
                _gameController.BonusForMoves();
                _victoryPanel.VictoryState();
            }
        }
    }
}