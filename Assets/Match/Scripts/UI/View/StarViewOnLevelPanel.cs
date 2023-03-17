using UnityEngine;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.UI.View
{

    public class StarViewOnLevelPanel : MonoBehaviour
    {
#if (UNITY_EDITOR)
        [RequiredField]
#endif
        [SerializeField] private LevelsConfigurationScriptable _levelConfig;

        [SerializeField] private GameObject[] _starPosition;
        [SerializeField] private GameObject _starPref;

        private void Awake()
        {
            CheckNumberOfStarsLevel();
        }

        private void CheckNumberOfStarsLevel()
        {
            GameObject[] totalStars = new GameObject[3];

            for (int i = 0; i < _levelConfig.level.totalStar; i++)
                totalStars[i] = Instantiate(_starPref, _starPosition[i].transform.position, Quaternion.identity, _starPosition[i].transform);
        }

    }
}
