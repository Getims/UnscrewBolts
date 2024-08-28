using Scripts.Configs.Sounds;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;

namespace Scripts.GameLogic.Sound
{
    public class SoundService : ISoundService
    {
        private readonly SoundPlayer _soundPlayer;
        private readonly AudioClipsListConfig _audioClipsListConfig;
        private readonly GlobalEventProvider _globalEventProvider;

        public bool IsSoundOn => _soundPlayer.IsSoundOn;
        public bool IsMusicOn => _soundPlayer.IsMusicOn;

        public SoundService(SoundPlayer soundPlayer, ISoundConfigProvider soundConfigProvider,
            GlobalEventProvider globalEventProvider)
        {
            _soundPlayer = soundPlayer;
            _audioClipsListConfig = soundConfigProvider.AudioClipsListConfig;

            _globalEventProvider = globalEventProvider;
            _globalEventProvider.Invoke<SoundSwitchEvent, bool>(_soundPlayer.IsSoundOn);
            _globalEventProvider.Invoke<MusicSwitchEvent, bool>(_soundPlayer.IsMusicOn);
        }

        public void SwitchSoundState(bool isOn)
        {
            _soundPlayer.SwitchSoundState();
            _globalEventProvider.Invoke<SoundSwitchEvent, bool>(isOn);
        }

        public void SwitchMusicState(bool isOn)
        {
            _soundPlayer.SwitchMusicState();
            _globalEventProvider.Invoke<MusicSwitchEvent, bool>(isOn);
        }

        public void PlayMenuBackgroundMusic() =>
            PlayMusic(_audioClipsListConfig.MenuBackgroundMusic);

        public void PlayGameBackgroundMusic() =>
            PlayMusic(_audioClipsListConfig.GameBackgroundMusic);

        public void PlayBoltChooseSound() =>
            PlaySound(_audioClipsListConfig.BoltChoose);

        public void PlayBoltPlaceSound() =>
            PlaySound(_audioClipsListConfig.BoltPlace);

        public void PlayBoltCantPlaceSound() =>
            PlaySound(_audioClipsListConfig.BoltCantPlace);

        public void PlayBoltDestroySound() =>
            PlaySound(_audioClipsListConfig.BoltDestroy);

        public void PlayGameElementHitSound() =>
            PlaySound(_audioClipsListConfig.GameElementHit);

        public void PlayButtonClickSound() =>
            PlaySound(_audioClipsListConfig.ButtonClick);

        public void PlayOpenMenuSound() =>
            PlaySound(_audioClipsListConfig.MenuOpen);

        public void PlayFailSound() =>
            PlaySound(_audioClipsListConfig.LevelFail);

        public void PlayWinSound() =>
            PlaySound(_audioClipsListConfig.LevelWin);

        public void PlayHardModeSound() =>
            PlaySound(_audioClipsListConfig.HardModeStart);

        public void PlayCoinsFlySound() =>
            PlaySound(_audioClipsListConfig.CoinsFly);

        public void PlayCoinsDropSound() =>
            PlaySound(_audioClipsListConfig.CoinsDrop);
        
        public void PlayBombExplosion() =>
            PlaySound(_audioClipsListConfig.BombExplosion);

        public void PlayOneShot(AudioClipConfig clipConfig) =>
            _soundPlayer.PlaySoundAndCreateAudioSource(clipConfig);

        private void PlayMusic(AudioClipConfig musicConfig) => _soundPlayer.PlayMusic(musicConfig);
        private void PlaySound(AudioClipConfig soundConfig) => _soundPlayer.PlaySound(soundConfig);
    }
}