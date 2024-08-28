using System;
using Scripts.Configs.Core;
using Scripts.Configs.Sounds;
using Scripts.Core.Enums;
using UnityEngine;

namespace Scripts.Configs
{
    [Serializable]
    public class GlobalConfig : ScriptableConfig
    {
        [SerializeField,]
        private StartSceneViewer _startScene = StartSceneViewer.Default;

        [SerializeField]
        private VolumeConfig _volumeConfig;

        [SerializeField]
        private AudioClipsListConfig _audioClipsListConfig;

        [SerializeField]
        private int _coinsPerGameElement = 2;

        [SerializeField]
        [Tooltip("Additional time when game over")]
        private int _additionalTime = 60;

        public VolumeConfig VolumeConfig => _volumeConfig;
        public AudioClipsListConfig AudioClipsListConfig => _audioClipsListConfig;
        public Scenes StartScene => (Scenes) _startScene;
        public int CoinsPerGameElement => _coinsPerGameElement;
        public int AdditionalTime => _additionalTime;

        private enum StartSceneViewer
        {
            Default = -1,
            MainMenu = 1,
            Game = 2
        }
    }
}