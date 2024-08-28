using System.Collections;
using System.Collections.Generic;
using Scripts.Configs.Sounds;
using Scripts.Core.Utilities;
using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Configs;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _gameplayAS;

        [SerializeField]
        private AudioSource _musicAS;

        private readonly List<GameSound> _interfaceSoundList = new List<GameSound>();
        private IPlayerDataService _playerDataService;
        private VolumeConfig _volumeConfig;
        private Coroutine _musicCoroutine;

        private bool _isSoundOn = true;
        private bool _isMusicOn = true;
        private float _lastBgPercent = 0;

        public bool IsSoundOn => _isSoundOn;
        public bool IsMusicOn => _isMusicOn;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, ISoundConfigProvider soundConfigProvider)
        {
            _playerDataService = playerDataService;
            _volumeConfig = soundConfigProvider.VolumeConfig;
        }

        public void SwitchSoundState()
        {
            _isSoundOn = !_isSoundOn;
            if (!_isSoundOn)
                _gameplayAS.Stop();

            _playerDataService.SetSoundState(_isSoundOn);
        }

        public void SwitchMusicState()
        {
            _isMusicOn = !_isMusicOn;

            if (_isMusicOn && _musicAS.enabled)
                _musicAS.Play();
            else
                _musicAS.Stop();

            _playerDataService.SetMusicState(_isMusicOn);
        }

        public void PlaySound(AudioClipConfig audioClipConfig, bool ignoreTime = true, float pauseTime = 1.0f)
        {
            if (!_isSoundOn)
                return;

            if (audioClipConfig.IsDisabled)
                return;

            bool canPlay = true;

            if (!ignoreTime)
            {
                GameSound gameSound = _interfaceSoundList.Find(s => s.Id == audioClipConfig.FileName);
                int timeNow = UnixTime.Now;

                if (gameSound == null)
                {
                    GameSound sound = new GameSound(audioClipConfig.FileName, timeNow);
                    _interfaceSoundList.Add(sound);
                }
                else
                {
                    canPlay = (timeNow - gameSound.LastPlayTime) >= pauseTime;

                    if (canPlay)
                        gameSound.SetLastPlayTime(timeNow);
                }
            }

            if (!canPlay)
                return;

            _gameplayAS.pitch = Random.Range(_volumeConfig.SoundMinPitch, _volumeConfig.SoundMaxPitch);
            _gameplayAS.PlayOneShot(audioClipConfig.AudioClip, GetCustomGameplaySoundVolume(audioClipConfig));
        }

        public void PlaySoundAndCreateAudioSource(AudioClipConfig audioClipConfig)
        {
            if (!_isSoundOn)
                return;

            if (audioClipConfig.IsDisabled)
                return;

            if (audioClipConfig.AudioClip == null)
                return;

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.pitch = Random.Range(0.95f, 1f);
            audioSource.priority = 100;
            audioSource.volume = audioClipConfig.SoundPercent;
            audioSource.PlayOneShot(audioClipConfig.AudioClip, GetCustomGameplaySoundVolume(audioClipConfig));

            Destroy(audioSource, audioClipConfig.AudioClip.length);
        }

        public AudioSource PlayLoopedSound(AudioClipConfig audioClipConfig)
        {
            if (!_isSoundOn)
                return null;

            if (audioClipConfig.IsDisabled)
                return null;

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.pitch = Random.Range(0.95f, 1.01f);
            audioSource.priority = 100;
            audioSource.loop = true;
            audioSource.clip = audioClipConfig.AudioClip;
            audioSource.volume = GetCustomGameplaySoundVolume(audioClipConfig);
            audioSource.Play();

            return audioSource;
        }

        public void DestroyAudioSource(AudioSource audioSource)
        {
            if (audioSource != null)
                Destroy(audioSource);
        }

        public void PlayMusic(AudioClipConfig AudioClipConfig, bool highPassEffect = false, int frequency = 1000)
        {
            if (AudioClipConfig == null)
            {
                Debug.LogWarning("No music config");
                return;
            }

            if (AudioClipConfig.IsDisabled)
                return;

            _musicAS.pitch = 1;
            EnableBackgroundHighPass(highPassEffect, frequency);

            if (_musicCoroutine != null)
                StopCoroutine(_musicCoroutine);

            _musicCoroutine =
                StartCoroutine(BackGroundMusicFading(AudioClipConfig.AudioClip, AudioClipConfig.SoundPercent));
        }

        private void Start()
        {
            _interfaceSoundList.Clear();
            _isSoundOn = _playerDataService.IsSoundOn;
            _isMusicOn = _playerDataService.IsMusicOn;
        }

        private float GetCustomGameplaySoundVolume(AudioClipConfig config) =>
            config == null
                ? _volumeConfig.SoundsVolume
                : _volumeConfig.SoundsVolume * config.SoundPercent;

        private void EnableBackgroundHighPass(bool state = false, int frequency = 1000)
        {
            AudioHighPassFilter audioHighPassFilter = _musicAS.GetComponent<AudioHighPassFilter>();
            audioHighPassFilter.cutoffFrequency = frequency;
            audioHighPassFilter.enabled = state;
        }

        private IEnumerator BackGroundMusicFading(AudioClip clip, float newPercent)
        {
            _musicAS.volume = _volumeConfig.MusicVolume * _lastBgPercent;
            float elapsedTime = 0;

            while (elapsedTime <= _volumeConfig.MusicFadeTime)
            {
                _musicAS.volume = Mathf.Lerp(_volumeConfig.MusicVolume * _lastBgPercent, 0,
                    elapsedTime / _volumeConfig.MusicFadeTime);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            elapsedTime = 0;
            _musicAS.volume = 0;
            _musicAS.Stop();
            _musicAS.clip = clip;

            if (_isMusicOn && _musicAS.enabled)
                _musicAS.Play();

            _lastBgPercent = newPercent;
            while (elapsedTime <= _volumeConfig.MusicFadeTime)
            {
                _musicAS.volume = Mathf.Lerp(0, _volumeConfig.MusicVolume * _lastBgPercent,
                    elapsedTime / _volumeConfig.MusicFadeTime);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _musicAS.volume = _volumeConfig.MusicVolume * _lastBgPercent;
        }
    }
}