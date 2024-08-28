using System;
using Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Sounds
{
    [Serializable]
    public class VolumeConfig : ScriptableConfig
    {
        [Title("Music")]
        [SerializeField]
        private float _musicFadeTime = 0.5f;

        [SerializeField]
        [Range(0, 1)]
        private float _musicVolume = 0.7f;

        [Title("Sounds")]
        [SerializeField]
        [Range(-3, 3)]
        private float _soundMinPitch = 0.9f;

        [SerializeField]
        [Range(-3, 3)]
        private float _soundMaxPitch = 1f;

        [SerializeField]
        [Range(0, 1)]
        private float _soundsVolume = 1f;

        public float MusicFadeTime => _musicFadeTime;
        public float MusicVolume => _musicVolume;
        public float SoundMinPitch => _soundMinPitch;
        public float SoundMaxPitch => _soundMaxPitch;
        public float SoundsVolume => _soundsVolume;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.SOUND_CATEGORY;
#endif
    }
}