using UnityEngine;

namespace Assets.Match.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance
        {
            get { return _instance; }
        }

        private static AudioManager _instance = null;

        private void Awake()
        {
            if (_instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}