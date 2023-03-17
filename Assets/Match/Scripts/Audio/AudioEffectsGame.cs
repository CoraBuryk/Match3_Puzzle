using UnityEngine;

namespace Assets.Match.Scripts.Audio
{

    public class AudioEffectsGame : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private AudioSource _dropSound;
        [SerializeField] private AudioSource _loseSound;
        [SerializeField] private AudioSource _victorySound;
        [SerializeField] private AudioSource _selectSound;
        [SerializeField] private AudioSource _bombSound;
        [SerializeField] private AudioSource _rocketSound;
        [SerializeField] private AudioSource _bonusSound;

#endregion

        public void PlayDropSound()
        {
            _dropSound.Play();
        }

        public void PlayLoseSound()
        {
            _loseSound.Play();
        }

        public void PlayVictorySound()
        {
            _victorySound.Play();
        }

        public void PlaySelectSound()
        {
            _selectSound.Play();
        }

        public void PlayBombSound()
        {
            _bombSound.Play();
        }

        public void PlayRocketSound()
        {
            _rocketSound.Play();
        }

        public void PlayBonusSound()
        {
            _bonusSound.Play();
        }

    }
}
