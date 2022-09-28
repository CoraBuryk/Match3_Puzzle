using System;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{
    public class ScoreController : MonoBehaviour
    {
        public int Counter { get; set; }
        public event Action ScoreChange;

        public int ScorePerBlock { get; } = 20;
        public float ScoreMultiplier { get; } = 1.2f;

        public void ChangeScore(int newScore)
        {
            Counter = newScore;
            ScoreChange?.Invoke();
        }
    }
}
