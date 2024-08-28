using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Sounds
{
    [Serializable]
    public class AudioClipConfig
    {
        [SerializeField]
        private bool _isDisabled;

        [SerializeField, Required, AssetsOnly]
        [HideIf(nameof(_isDisabled))]
        private AudioClip _audioClip;

        [SerializeField, Range(0, 1)]
        [HideIf(nameof(_isDisabled))]
        private float _soundPercent = 1;

        public bool IsDisabled => _isDisabled;
        public AudioClip AudioClip => _audioClip;
        public float SoundPercent => _soundPercent;
        public string FileName => _audioClip.GetHashCode().ToString();
    }
}