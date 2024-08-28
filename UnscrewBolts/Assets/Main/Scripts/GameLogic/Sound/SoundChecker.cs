using System.Collections.Generic;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;
using AudioType = Scripts.Core.Enums.AudioType;

namespace Scripts.GameLogic.Sound
{
    public class SoundChecker : MonoBehaviour
    {
        [SerializeField]
        private AudioType _audioType = AudioType.Sound;

        [SerializeField]
        private List<AudioSource> _audioSources = new List<AudioSource>();

        [SerializeField]
        private bool _onlyCheckOnStart = true;

        private ISoundService _soundService;
        private GlobalEventProvider _globalEventProvider;

        [Inject]
        public void Construct(ISoundService soundService, GlobalEventProvider globalEventProvider)
        {
            _globalEventProvider = globalEventProvider;
            _soundService = soundService;
        }

        private void Start()
        {
            if (_soundService == null)
            {
                Debug.LogWarning("No sound service!");
                return;
            }

            if (_audioType == AudioType.Sound)
            {
                SetState(_soundService.IsSoundOn);

                if (!_onlyCheckOnStart)
                    _globalEventProvider?.AddListener<SoundSwitchEvent, bool>(OnSwitchSound);
            }
            else
            {
                SetState(_soundService.IsMusicOn);

                if (!_onlyCheckOnStart)
                    _globalEventProvider?.AddListener<MusicSwitchEvent, bool>(OnSwitchSound);
            }
        }

        private void OnDestroy()
        {
            if (_audioType == AudioType.Sound)
            {
                if (!_onlyCheckOnStart)
                    _globalEventProvider?.RemoveListener<SoundSwitchEvent, bool>(OnSwitchSound);
            }
            else
            {
                if (!_onlyCheckOnStart)
                    _globalEventProvider?.RemoveListener<MusicSwitchEvent, bool>(OnSwitchSound);
            }
        }

        private void OnSwitchSound(bool isOn) =>
            SetState(isOn);

        private void SetState(bool isOn)
        {
            foreach (var audioSource in _audioSources)
                audioSource.mute = !isOn;
        }
    }
}