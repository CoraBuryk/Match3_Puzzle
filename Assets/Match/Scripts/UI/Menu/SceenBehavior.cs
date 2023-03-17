using UnityEngine;
using UnityEngine.UI;
using Assets.Match.Scripts.Gameplay;
using System.Threading.Tasks;

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

        private bool _isGoalReached = false;

        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(_pausePanel.PauseGame);
            _moveController.MovesChange += GameOver;
            _goalController.GoalChange += GoalReached;
        }

        public async void GameOver()
        {
            await Victory();

            if (_moveController.TotalMove <= 0 && _isGoalReached == false)
            {
                _gameOverPanel.GameOverState();
            }
        }

        public void GoalReached()
        {
            if (_goalController.CounterOne <= 0 && _goalController.CounterTwo <= 0 && _goalController.CounterThree <= 0
                && _moveController.TotalMove >= 0)
            {
                _isGoalReached = true;
                _gameController.BonusForMoves();
                _victoryPanel.VictoryState();
            }
        }

        public Task Victory()
        {
            GoalReached();
            return Task.CompletedTask;
        }

        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(_pausePanel.PauseGame);
            _moveController.MovesChange -= GameOver;
            _goalController.GoalChange -= GoalReached;
        }

    }
}
