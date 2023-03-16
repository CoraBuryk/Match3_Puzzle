using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if (UNITY_EDITOR)
using Assets.Match.Scripts.EditorChanges;
#endif
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.UI
{
    public class NameConnecter : MonoBehaviour
    {
#if (UNITY_EDITOR)
        [RequiredField]
#endif
        [SerializeField] private LevelsConfigurationScriptable _levelConfig;
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