using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ScoreController _scoreController;

        public void CountingScore(int totalBlock)
        {
            float score = 0f;
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
    }
}
