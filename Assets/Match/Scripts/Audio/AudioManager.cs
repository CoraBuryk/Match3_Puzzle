using UnityEngine;
using Assets.Match.Scripts.InputSystemController;
using Assets.Match.Scripts.ScriptableObjects;

namespace Assets.Match.Scripts.Audio
{

    public class AudioManager : SingletonPersistent<AudioManager>
    {
        [SerializeField] private AudioScriptableObject _audioController;
        private AudioSource _backgroundAudio;

        public override void Awake()
        {
            base.Awake();
            _backgroundAudio = GetComponent<AudioSource>();

            if(_backgroundAudio.enabled)
            {
                _backgroundAudio.Play();
                _audioController.Master.masterVolume = _backgroundAudio.volume;
            }
            
        }

    }
}