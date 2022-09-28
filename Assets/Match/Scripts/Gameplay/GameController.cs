using Assets.Match.Scripts.Models;
using Assets.Match.Scripts.ScriptableObjects;
using Assets.Match.Scripts.UI.View;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private GoalView _goalView;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private StarController _starController;

        [SerializeField] private GameObject _goalObjectOne;
        [SerializeField] private GameObject _goalObjectTwo;
        [SerializeField] private GameObject _goalObjectThree;

        [SerializeField] private BoardManager _boardManager;
        #endregion

        public void CountingScore(int totalBlock)
        {
            float score = 0f;
            if(totalBlock < 3)
            {
                _scoreController.ChangeScore(_scoreController.Counter);
            }
            if (totalBlock >= 3 && totalBlock < 5)
            {
                score = _scoreController.ScorePerBlock * totalBlock;
                _scoreController.ChangeScore(_scoreController.Counter + (int)score);
            }
            if(totalBlock >= 5 && totalBlock <= 7)
            {
                score = _scoreController.ScorePerBlock * totalBlock * _scoreController.ScoreMultiplier;
                _scoreController.ChangeScore(_scoreController.Counter + (int)score);
            }
            if(totalBlock >= 8)
            {                
                score = _scoreController.ScorePerBlock * totalBlock * _scoreController.ScoreMultiplier * _scoreController.ScoreMultiplier;
                _scoreController.ChangeScore(_scoreController.Counter + (int)score);
            }
        }

        public void BonusForMoves()
        {
            if(_moveController.TotalMove > 0)
            {
                _scoreController.ChangeScore(_scoreController.Counter + _moveController.TotalMove * 100);
            }
        }

        public void CheckNumberOfGoals()
        {
            if(_goalController.CounterOne <= 0)
            {
                _goalObjectOne.SetActive(false);
            }
            if (_goalController.CounterTwo <= 0)
            {
                _goalObjectTwo.SetActive(false);
            }
            if (_goalController.CounterThree <= 0)
            {
                _goalObjectThree.SetActive(false);
            }
        }

        public void MatchWithGoal(Block block)
        {
            if (block.BlockRenderer.sprite == _goalView.goalImage[0].sprite)
            {
                _goalController.ChangeGoalOne(_goalController.CounterOne - 1);
            }
            if (block.BlockRenderer.sprite == _goalView.goalImage[1].sprite)
            {
                _goalController.ChangeGoalTwo(_goalController.CounterTwo - 1);
            }
            if (block.BlockRenderer.sprite == _goalView.goalImage[2].sprite)
            {
                _goalController.ChangeGoalThree(_goalController.CounterThree - 1);
            }

            CheckNumberOfGoals();
        }

        public void Restart()
        {
            _scoreController.ChangeScore(0);
            _starController.ResetStar();
            LevelControl();
            CheckNumberOfGoals();
            _goalObjectOne.SetActive(true);
            _goalObjectTwo.SetActive(true);
            _goalObjectThree.SetActive(true);
        }

        public void LevelControl()
        {
            _goalController.ResetGoals();
            _moveController.ResetMoves();
        }
    }
}
