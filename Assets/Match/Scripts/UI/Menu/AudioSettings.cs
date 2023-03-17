using UnityEngine;
using UnityEngine.UI;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.UI.Menu
{

    public class AudioSettings : MonoBehaviour
    {

#region Serialized Variables

        [SerializeField] private AudioScriptableObject _audioController;
        [SerializeField] private ButtonAudioEffect _buttonClickEffect;

        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _muteMusicButton;
        [SerializeField] private Button _muteSoundsButton;

#endregion

        private void OnEnable()
        {
            _muteMusicButton.onClick.AddListener(MuteMusic);
            _muteSoundsButton.onClick.AddListener(MuteSounds);
            _volumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        }

        private void Start()
        {
            LoadAudio();
        }

        private void MuteSounds()
        {
            if(_audioController.Sounds.mutedSound == false)
            {
                _audioController.Sounds.soundsVolume = 0f;
                _audioController.masterGroup.audioMixer.SetFloat("Sounds", -80f);
                _muteSoundsButton.image.sprite = _audioController.Sounds.soundsOff;
                _audioController.Sounds.mutedSound = true; 
            }
            else if(_audioController.Sounds.mutedSound == true)
            {
                _audioController.Sounds.soundsVolume = 1f;
                _audioController.masterGroup.audioMixer.SetFloat("Sounds", 0f);
                _muteSoundsButton.image.sprite = _audioController.Sounds.soundsOn;
                _audioController.Sounds.mutedSound = false;
            }
            _buttonClickEffect.PlayClickSound();
        }

        private void MuteMusic()
        {
            if (_audioController.Music.mutedMusic == false)
            {
                _audioController.Music.musicVolume = 0f;
                _audioController.masterGroup.audioMixer.SetFloat("Music", -80f);
                _muteMusicButton.image.sprite = _audioController.Music.musicOff;
                _audioController.Music.mutedMusic = true;
            }
            else if (_audioController.Music.mutedMusic == true)
            {
                _audioController.Music.musicVolume = 1f;
                _audioController.masterGroup.audioMixer.SetFloat("Music", 0f);
                _muteMusicButton.image.sprite = _audioController.Music.musicOn;
                _audioController.Music.mutedMusic = false;
            }
            _buttonClickEffect.PlayClickSound();
        }

        private void LoadAudio()
        {
            if (_audioController.Sounds.soundsVolume == 0f && _audioController.Sounds.mutedSound == true)
            {
                _audioController.masterGroup.audioMixer.SetFloat("Sounds", -80f);
                _muteSoundsButton.image.sprite = _audioController.Sounds.soundsOff;
            }
            else if(_audioController.Sounds.soundsVolume == 1f && _audioController.Sounds.mutedSound == false)
            {
                _audioController.masterGroup.audioMixer.SetFloat("Sounds", 0f);
                _muteSoundsButton.image.sprite = _audioController.Sounds.soundsOn;
            }

            if (_audioController.Music.musicVolume == 0f && _audioController.Music.mutedMusic == true)
            {
                _audioController.masterGroup.audioMixer.SetFloat("Music", -80f);
                _muteMusicButton.image.sprite = _audioController.Music.musicOff;
            }
            else if (_audioController.Sounds.soundsVolume == 1f && _audioController.Sounds.mutedSound == false)
            {
                _audioController.masterGroup.audioMixer.SetFloat("Music", 0f);
                _muteMusicButton.image.sprite = _audioController.Music.musicOn;
            }

            _audioController.masterGroup.audioMixer.SetFloat("MasterVolume", ConvertToDecibel(_audioController.Master.masterVolume));
            _volumeSlider.value = _audioController.Master.masterVolume;
        }

        private void ChangeMasterVolume(float volume)
        {
            AudioListener.volume = _volumeSlider.value;
            SaveMasterAudio();
        }

        private void SaveMasterAudio()
        {
            _audioController.Master.masterVolume = _volumeSlider.value;
            _audioController.masterGroup.audioMixer.SetFloat("MasterVolume", ConvertToDecibel(_audioController.Master.masterVolume));
        }

        private float ConvertToDecibel(float _value)
        {
            return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 40f;
        }

        private void OnDisable()
        {
            _muteMusicButton.onClick.RemoveListener(MuteMusic);
            _muteSoundsButton.onClick.RemoveListener(MuteSounds);
            _volumeSlider.onValueChanged.RemoveListener(ChangeMasterVolume);
        }

    }
}
