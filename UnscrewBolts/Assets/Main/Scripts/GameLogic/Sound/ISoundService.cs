using Scripts.Configs.Sounds;

namespace Scripts.GameLogic.Sound
{
    public interface ISoundService
    {   void SwitchSoundState(bool isOn);
        void SwitchMusicState(bool isOn);
        bool IsSoundOn { get; }
        bool IsMusicOn { get; }
        void PlayOneShot(AudioClipConfig clipConfig);
        
        void PlayMenuBackgroundMusic();
        void PlayGameBackgroundMusic();
        
        void PlayBoltChooseSound();
        void PlayBoltPlaceSound();
        void PlayBoltCantPlaceSound();
        void PlayBoltDestroySound();
        void PlayGameElementHitSound();
        
        void PlayButtonClickSound();
        void PlayOpenMenuSound();
        void PlayFailSound();
        void PlayWinSound();
        void PlayHardModeSound();
        void PlayCoinsFlySound();
        void PlayCoinsDropSound();
        void PlayBombExplosion();
    }
}