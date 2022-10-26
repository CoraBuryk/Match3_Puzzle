using System;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{

    public class ScoreController : MonoBehaviour
    {
        public int Counter { get; private set; }       

        public int ScorePerBlock { get; } = 20;

        public float ScoreMultiplier { get; } = 1.2f;

        public event Action ScoreChange;

        public void ChangeScore(int newScore)
        {
            Counter = newScore;
            ScoreChange?.Invoke();
        }

    }
}
