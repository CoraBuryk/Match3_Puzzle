using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Match.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Audio")]

    public class AudioScriptableObject : ScriptableObject
    {
        public AudioMixerGroup masterGroup;

        public SoundsMixerGroup Sounds;
        public MusicMixerGroup Music;
        public MasterMixerGroup Master;

        [Serializable]
        public struct SoundsMixerGroup
        {
            public Sprite soundsOff;
            public Sprite soundsOn;
            public bool mutedSound;
            [Range(0, 1f)] public float soundsVolume;
        }

        [Serializable]
        public struct MusicMixerGroup
        {
            public Sprite musicOff;
            public Sprite musicOn;
            public bool mutedMusic;
            [Range(0, 1f)] public float musicVolume;
        }

        [Serializable]
        public struct MasterMixerGroup
        {
            [Range(0.001f, 1f)] public float masterVolume;
        }

    }
}
