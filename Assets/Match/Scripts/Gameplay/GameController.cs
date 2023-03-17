using UnityEngine;
using Assets.Match.Scripts.UI.View;

namespace Assets.Match.Scripts.Gameplay
{

    public class GameController : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private GoalController _goalController;
        [SerializeField] private MoveController _moveController;
        [SerializeField] private StarController _starController;
        [SerializeField] private GoalView _goalView;

        [SerializeField] private GameObject[] _goalObject;

#endregion

        public void CountingScore(int totalBlock)
        {
            float score;
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
                _goalObject[0].SetActive(false);
            }

            if (_goalController.CounterTwo <= 0)
            {
                _goalObject[1].SetActive(false);
            }

            if (_goalController.CounterThree <= 0)
            {
                _goalObject[2].SetActive(false);
            }
        }

        public void MatchWithGoal(BlockController block)
        {
            if (block.Type == _goalView.goals[0].GetComponent<BlockController>().Type)
            {
                _goalController.ChangeGoals(_goalController.CounterOne - 1, 1);
            }

            if (block.Type == _goalView.goals[1].GetComponent<BlockController>().Type)
            {
                _goalController.ChangeGoals(_goalController.CounterTwo - 1, 2);
            }

            if (block.Type == _goalView.goals[2].GetComponent<BlockController>().Type)
            {
                _goalController.ChangeGoals(_goalController.CounterThree - 1, 3);
            }

            CheckNumberOfGoals();
        }

        public void Restart()
        {
            _scoreController.ChangeScore(0);           
            LevelControl();
            CheckNumberOfGoals();

            for(int i = 0; i < _goalObject.Length; i++)
            {
                _goalObject[i].SetActive(true);
            }
        }

        public void LevelControl()
        {
            _goalController.ResetGoals();
            _moveController.ResetMoves();
        }
    }
}
