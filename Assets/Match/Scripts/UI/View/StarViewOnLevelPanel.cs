using UnityEngine;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.UI.View
{

    public class StarViewOnLevelPanel : MonoBehaviour
    {
        [SerializeField] private LevelScriptableObject _levelScriptableObject;

        [SerializeField] private GameObject[] _starPosition;
        [SerializeField] private GameObject _starPref;

        private void Awake()
        {
            CheckNumberOfStarsLevel();
        }

        private void CheckNumberOfStarsLevel()
        {
            GameObject[] totalStars = new GameObject[3];

            if (_levelScriptableObject.totalStar == 1)
            {
                totalStars[0] = Instantiate(_starPref, _starPosition[0].transform.position, Quaternion.identity, _starPosition[0].transform);          
            }

            if (_levelScriptableObject.totalStar == 2)
            {
                totalStars[0] = Instantiate(_starPref, _starPosition[0].transform.position, Quaternion.identity, _starPosition[0].transform);
                totalStars[1] = Instantiate(_starPref, _starPosition[1].transform.position, Quaternion.identity, _starPosition[1].transform);         
            }

            if (_levelScriptableObject.totalStar == 3)
            {
                totalStars[0] = Instantiate(_starPref, _starPosition[0].transform.position, Quaternion.identity, _starPosition[0].transform);
                totalStars[1] = Instantiate(_starPref, _starPosition[1].transform.position, Quaternion.identity, _starPosition[1].transform);
                totalStars[2] = Instantiate(_starPref, _starPosition[2].transform.position, Quaternion.identity, _starPosition[2].transform);
            }
        }

    }
}
