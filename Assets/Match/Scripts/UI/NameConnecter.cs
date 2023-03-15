using Assets.Match.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Match.Scripts.UI
{
    public class NameConnecter : MonoBehaviour
    {
        [SerializeField, RequiredField] private LevelsConfigurationScriptable _levelConfig;
        [SerializeField] private TextMeshPro _levelNumberText;
        [SerializeField] private GameObject _lockImage;

        private void Awake()
        {
            if (_levelConfig != null)
            {
                _lockImage.SetActive(false);
                _levelNumberText.text = _levelConfig.levelNumber.ToString();
            }
            else
            {
                _lockImage.SetActive(true);
                GetComponent<Button>().interactable = false;
            }

        }
    }
}