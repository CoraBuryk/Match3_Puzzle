using UnityEngine;

namespace Assets.Match.Scripts.Audio
{

    public class ButtonAudioEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource _buttonClick;

        public void PlayClickSound()
        {
            _buttonClick.Play();
        }

    }
}
