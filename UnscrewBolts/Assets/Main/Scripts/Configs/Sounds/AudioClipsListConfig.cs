using System;
using Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs.Sounds
{
    [Serializable]
    public class AudioClipsListConfig : ScriptableConfig
    {
        [Title("Background")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_menuBackgroundMusic) + ")")]
        private AudioClipConfig _menuBackgroundMusic;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_gameBackgroundMusic) + ")")]
        private AudioClipConfig _gameBackgroundMusic;

        [Title("Game")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_boltChoose) + ")")]
        private AudioClipConfig _boltChoose;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_boltPlace) + ")")]
        private AudioClipConfig _boltPlace;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_boltCantPlace) + ")")]
        private AudioClipConfig _boltCantPlace;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_boltDestroy) + ")")]
        private AudioClipConfig _boltDestroy;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_gameElementHit) + ")")]
        private AudioClipConfig _gameElementHit;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_bombExplosion) + ")")]
        private AudioClipConfig _bombExplosion;

        [Title("UI")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_buttonClick) + ")")]
        private AudioClipConfig _buttonClick;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_menuOpen) + ")")]
        private AudioClipConfig _menuOpen;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_levelFail) + ")")]
        private AudioClipConfig _levelFail;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_levelWin) + ")")]
        private AudioClipConfig _levelWin;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_hardModeStart) + ")")]
        private AudioClipConfig _hardModeStart;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_coinsFly) + ")")]
        private AudioClipConfig _coinsFly;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_coinsDrop) + ")")]
        private AudioClipConfig _coinsDrop;

        public AudioClipConfig MenuBackgroundMusic => _menuBackgroundMusic;
        public AudioClipConfig GameBackgroundMusic => _gameBackgroundMusic;

        public AudioClipConfig BoltChoose => _boltChoose;
        public AudioClipConfig BoltPlace => _boltPlace;
        public AudioClipConfig BoltCantPlace => _boltCantPlace;
        public AudioClipConfig BoltDestroy => _boltDestroy;
        public AudioClipConfig GameElementHit => _gameElementHit;

        public AudioClipConfig ButtonClick => _buttonClick;
        public AudioClipConfig MenuOpen => _menuOpen;
        public AudioClipConfig LevelFail => _levelFail;
        public AudioClipConfig LevelWin => _levelWin;
        public AudioClipConfig HardModeStart => _hardModeStart;
        public AudioClipConfig CoinsFly => _coinsFly;
        public AudioClipConfig CoinsDrop => _coinsDrop;
        public AudioClipConfig BombExplosion => _bombExplosion;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.SOUND_CATEGORY;

        private string GetLabel(AudioClipConfig clipConfig)
        {
            if (clipConfig.IsDisabled)
                return "- Disabled";
            if (clipConfig.AudioClip == null)
                return "- No Audio";
            return clipConfig.AudioClip.name;
        }
#endif
    }
}