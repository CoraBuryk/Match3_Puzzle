using Assets.Match.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.Match.Scripts.Gameplay
{

    public class StarController : MonoBehaviour
    {
        [SerializeField, RequiredField] private LevelsConfigurationScriptable _levelConfig;    

        public int NumOfStar { get; set; }

        public event Action StarChange;

        private int _minStar;

        private void Awake()
        {
            NumOfStar = _minStar;
        }

        public void StarIncrease(int minStar)
        {           
            NumOfStar = minStar;
            StarChange?.Invoke();
        }

        public void ResetStar()
        {
            _minStar = 0;
            NumOfStar = _minStar;
            StarChange?.Invoke();
        }

        public void SaveStarData()
        {
            _levelConfig.level.totalStar = NumOfStar;
        }

    }
}
