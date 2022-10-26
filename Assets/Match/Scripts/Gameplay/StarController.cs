using System;
using UnityEngine;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Gameplay
{

    public class StarController : MonoBehaviour
    {
        [SerializeField] private LevelScriptableObject _levelScriptableObject;       

        public int NumOfStar { get; set; }

        public event Action StarChange;

        private int _minStar = 0;

        private void Awake()
        {
            NumOfStar = _minStar;
        }

        public void StarIncrease(int minStar)
        {           
            NumOfStar = minStar;
            _levelScriptableObject.totalStar = NumOfStar;
            StarChange?.Invoke();
        }

        public void ResetStar()
        {
            _minStar = 0;
            NumOfStar = _minStar;
            _levelScriptableObject.totalStar = NumOfStar;
            StarChange?.Invoke();
        }

    }
}
